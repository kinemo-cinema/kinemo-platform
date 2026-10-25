using Cortex.Mediator;
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
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IDashboardCommandService
{
    public async Task<Dashboard?> Handle(GenerateDashboardCommand command)
    {
        var dashboard = new Dashboard(command);

        // SIMULATION: In a real system, you would query BC05, BC06, and BC07 
        // to get the actual data. Here, we mock some analytics for demonstration.
        
        dashboard.AddMetric(new OperationalMetric(EMetricType.RoomUtilization, 85.5m, command.PeriodStart, command.PeriodEnd));
        dashboard.AddMetric(new OperationalMetric(EMetricType.ShowCount, 120m, command.PeriodStart, command.PeriodEnd));
        
        dashboard.AddRoomPerformance(new RoomPerformance(1, 45.5m, 60, 2));
        dashboard.AddRoomPerformance(new RoomPerformance(2, 38.0m, 60, 0));
        
        dashboard.AddIncidentSummary(new IncidentSummary(5, 2, 1, 1, 1));

        await dashboardRepository.AddAsync(dashboard);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new DashboardGeneratedEvent(dashboard.Id, dashboard.PeriodStart, dashboard.PeriodEnd));

        return dashboard;
    }
}