using KinemoPlatform.API.Maintenance.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Maintenance.Application.Internal.EventHandlers;

public class EquipmentRegisteredEventHandler : IEventHandler<EquipmentRegisteredEvent>
{
    public Task Handle(EquipmentRegisteredEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine("EQUIPMENT REGISTERED: ID {0} in Room {1}", domainEvent.EquipmentId, domainEvent.RoomId);
        return Task.CompletedTask;
    }
}
