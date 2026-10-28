using KinemoPlatform.API.Execution.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Execution.Application.Internal.EventHandlers;

public class ExecutionFinishedEventHandler : IEventHandler<ExecutionFinishedEvent>
{
    public Task Handle(ExecutionFinishedEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine("EXECUTION FINISHED: ID {0} concluded successfully.", domainEvent.ExecutionId);
        return Task.CompletedTask;
    }
}