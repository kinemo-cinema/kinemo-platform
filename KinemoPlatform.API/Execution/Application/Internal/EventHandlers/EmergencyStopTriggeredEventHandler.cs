using KinemoPlatform.API.Execution.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Execution.Application.Internal.EventHandlers;

public class EmergencyStopTriggeredEventHandler : IEventHandler<EmergencyStopTriggeredEvent>
{
    public Task Handle(EmergencyStopTriggeredEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine("EMERGENCY STOP: Execution {0} stopped! Reason: {1}", domainEvent.ExecutionId, domainEvent.Reason);
        return Task.CompletedTask;
    }
}