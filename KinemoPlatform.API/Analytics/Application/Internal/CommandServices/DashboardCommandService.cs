using KinemoPlatform.API.Scheduling.Domain.Repositories;
using KinemoPlatform.API.Scheduling.Domain.Model.ValueObjects;
using KinemoPlatform.API.Rooms.Domain.Repositories;
using KinemoPlatform.API.Execution.Domain.Repositories;
﻿using Cortex.Mediator;
using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Commands;
using KinemoPlatform.API.Analytics.Domain.Model.Entities;
using KinemoPlatform.API.Analytics.Domain.Model.Events;
using KinemoPlatform.API.Analytics.Domain.Model.ValueObjects;
using KinemoPlatform.API.Analytics.Domain.Repositories;
using KinemoPlatform.API.Analytics.Domain.Services;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Analytics.Application.Internal.CommandServices;

public class DashboardCommandService(
    IDashboardRepository dashboardRepository,
    IShowRepository showRepository,
    IRoomRepository roomRepository,
    IExecutionRepository executionRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IDashboardCommandService
{
    public async Task<Dashboard?> Handle(GenerateDashboardCommand command)
    {
        var dashboard = new Dashboard(command);

        var shows = (await showRepository.ListAsync()).Where(show =>
            show.Status != EShowStatus.Cancelled &&
            show.TimeSlot.StartDateTime < command.PeriodEnd &&
            show.TimeSlot.EndDateTime > command.PeriodStart).ToList();
        var rooms = (await roomRepository.ListAsync()).ToList();
        var executions = (await executionRepository.ListWithDetailsAsync()).ToList();
        decimal Hours(KinemoPlatform.API.Scheduling.Domain.Model.Aggregates.Show show)
        {
            var start = show.TimeSlot.StartDateTime > command.PeriodStart ? show.TimeSlot.StartDateTime : command.PeriodStart;
            var end = show.TimeSlot.EndDateTime < command.PeriodEnd ? show.TimeSlot.EndDateTime : command.PeriodEnd;
            return (decimal)(end - start).TotalHours;
        }
        var hours = shows.Sum(Hours);
        var capacityHours = rooms.Count * (decimal)(command.PeriodEnd - command.PeriodStart).TotalHours;
        dashboard.AddMetric(new OperationalMetric(EMetricType.ShowCount, shows.Count, command.PeriodStart, command.PeriodEnd));
        dashboard.AddMetric(new OperationalMetric(EMetricType.OperatingHours, hours, command.PeriodStart, command.PeriodEnd));
        dashboard.AddMetric(new OperationalMetric(EMetricType.RoomUtilization,
            capacityHours > 0 ? Math.Min(100m, hours / capacityHours * 100m) : 0m,
            command.PeriodStart, command.PeriodEnd));
        foreach (var room in rooms)
        {
            var roomShows = shows.Where(show => show.RoomId == room.Id).ToList();
            var showIds = roomShows.Select(show => show.Id).ToHashSet();
            var emergencies = executions.Where(execution => showIds.Contains(execution.ShowId))
                .SelectMany(execution => execution.EmergencyEvents)
                .Count(emergency => emergency.TriggeredAt >= command.PeriodStart && emergency.TriggeredAt < command.PeriodEnd);
            dashboard.AddRoomPerformance(new RoomPerformance(room.Id, roomShows.Sum(Hours), roomShows.Count, emergencies));
        }

        await dashboardRepository.AddAsync(dashboard);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new DashboardGeneratedEvent(dashboard.Id, dashboard.PeriodStart, dashboard.PeriodEnd));

        return dashboard;
    }
}