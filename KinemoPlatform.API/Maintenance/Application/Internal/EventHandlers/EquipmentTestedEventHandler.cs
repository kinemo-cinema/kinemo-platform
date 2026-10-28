using KinemoPlatform.API.Maintenance.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Maintenance.Application.Internal.EventHandlers;

public class EquipmentTestedEventHandler : IEventHandler<EquipmentTestedEvent>
{
    public Task Handle(EquipmentTestedEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine("EQUIPMENT TESTED: Equipment {0}, Channel {1}, Passed: {2}", 
            domainEvent.EquipmentId, domainEvent.Channel, domainEvent.IsSuccessful);
        return Task.CompletedTask;
    }
}