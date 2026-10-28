using KinemoPlatform.API.Maintenance.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Maintenance.Application.Internal.EventHandlers;

public class MaintenanceCompletedEventHandler : IEventHandler<MaintenanceCompletedEvent>
{
    public Task Handle(MaintenanceCompletedEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine("MAINTENANCE COMPLETED: Order {0} for Equipment {1}", domainEvent.MaintenanceOrderId, domainEvent.EquipmentId);
        return Task.CompletedTask;
    }
}