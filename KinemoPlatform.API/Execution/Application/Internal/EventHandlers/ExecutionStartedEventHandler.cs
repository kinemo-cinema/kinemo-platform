using KinemoPlatform.API.Execution.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Execution.Application.Internal.EventHandlers;

public class ExecutionStartedEventHandler : IEventHandler<ExecutionStartedEvent>
{
    public Task Handle(ExecutionStartedEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine("EXECUTION STARTED: ID {0} for Show {1}", domainEvent.ExecutionId, domainEvent.ShowId);
        return Task.CompletedTask;
    }
}