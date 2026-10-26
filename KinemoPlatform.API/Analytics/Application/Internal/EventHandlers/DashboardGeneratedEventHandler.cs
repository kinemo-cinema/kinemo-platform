using KinemoPlatform.API.Analytics.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Analytics.Application.Internal.EventHandlers;

public class DashboardGeneratedEventHandler : IEventHandler<DashboardGeneratedEvent>
{
    public Task Handle(DashboardGeneratedEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine("DASHBOARD GENERATED: ID {0} for period {1:d} to {2:d}", 
            domainEvent.DashboardId, domainEvent.PeriodStart, domainEvent.PeriodEnd);
        return Task.CompletedTask;
    }
}