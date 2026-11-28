using Cortex.Mediator;
using KinemoPlatform.API.Analytics.Application.Errors;
using KinemoPlatform.API.Analytics.Application.Services;
using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Commands;
using KinemoPlatform.API.Analytics.Domain.Model.Entities;
using KinemoPlatform.API.Analytics.Domain.Model.Events;
using KinemoPlatform.API.Analytics.Domain.Model.ValueObjects;
using KinemoPlatform.API.Analytics.Domain.Repositories;
using KinemoPlatform.API.Execution.Interfaces.ACL;
using KinemoPlatform.API.Rooms.Interfaces.ACL;
using KinemoPlatform.API.Scheduling.Interfaces.ACL;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Analytics.Application.Internal.CommandServices;

public class DashboardCommandService(
    IDashboardRepository dashboardRepository,
    ISchedulingContextFacade schedulingFacade,
    IRoomsContextFacade roomsFacade,
    IExecutionContextFacade executionFacade,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IDashboardCommandService
{
    public async Task<Result<Dashboard, EAnalyticsError>> Handle(
        GenerateDashboardCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.PeriodStart >= command.PeriodEnd)
            return new Result<Dashboard, EAnalyticsError>.Failure(
                EAnalyticsError.InvalidPeriod);

        var dashboard = new Dashboard(command);

        var shows = await schedulingFacade.FetchActiveShowSlots(command.PeriodStart, command.PeriodEnd, cancellationToken);
        var roomIds = await roomsFacade.FetchRoomIds(cancellationToken);
        var emergencies = await executionFacade.FetchEmergencies(command.PeriodStart, command.PeriodEnd, cancellationToken);

        decimal Hours(ShowSlot show)
        {
            var start = show.StartDateTime > command.PeriodStart ? show.StartDateTime : command.PeriodStart;
            var end = show.EndDateTime < command.PeriodEnd ? show.EndDateTime : command.PeriodEnd;
            return (decimal)(end - start).TotalHours;
        }

        var hours = shows.Sum(Hours);
        var capacityHours = roomIds.Count * (decimal)(command.PeriodEnd - command.PeriodStart).TotalHours;
        dashboard.AddMetric(new OperationalMetric(EMetricType.ShowCount, shows.Count, command.PeriodStart, command.PeriodEnd));
        dashboard.AddMetric(new OperationalMetric(EMetricType.OperatingHours, hours, command.PeriodStart, command.PeriodEnd));
        dashboard.AddMetric(new OperationalMetric(EMetricType.RoomUtilization,
            capacityHours > 0 ? Math.Min(100m, hours / capacityHours * 100m) : 0m,
            command.PeriodStart, command.PeriodEnd));

        foreach (var roomId in roomIds)
        {
            var roomShows = shows.Where(show => show.RoomId == roomId).ToList();
            var showIds = roomShows.Select(show => show.ShowId).ToHashSet();
            var emergencyCount = emergencies.Count(emergency => showIds.Contains(emergency.ShowId));
            dashboard.AddRoomPerformance(new RoomPerformance(roomId, roomShows.Sum(Hours), roomShows.Count, emergencyCount));
        }

        await dashboardRepository.AddAsync(dashboard, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new DashboardGeneratedEvent(
                dashboard.Id,
                dashboard.PeriodStart,
                dashboard.PeriodEnd),
            cancellationToken);

        return new Result<Dashboard, EAnalyticsError>.Success(dashboard);
    }
}