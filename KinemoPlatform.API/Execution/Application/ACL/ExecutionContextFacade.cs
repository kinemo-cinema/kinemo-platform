using KinemoPlatform.API.Execution.Application.Services;
using KinemoPlatform.API.Execution.Domain.Model.Queries;
using KinemoPlatform.API.Execution.Interfaces.ACL;

namespace KinemoPlatform.API.Execution.Application.ACL;

public class ExecutionContextFacade(IShowExecutionQueryService executionQueryService) : IExecutionContextFacade
{
    public async Task<IReadOnlyList<EmergencyOccurrence>> FetchEmergencies(
        DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default)
    {
        var executions = await executionQueryService.Handle(new GetAllExecutionsQuery(), cancellationToken);
        return executions
            .SelectMany(execution => execution.EmergencyEvents
                .Where(emergency => emergency.TriggeredAt >= from && emergency.TriggeredAt < to)
                .Select(emergency => new EmergencyOccurrence(execution.ShowId, emergency.TriggeredAt)))
            .ToList();
    }
}