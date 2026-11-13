using KinemoPlatform.API.Execution.Application.Services;
using KinemoPlatform.API.Execution.Domain.Model.Aggregates;
using KinemoPlatform.API.Execution.Domain.Model.Queries;
using KinemoPlatform.API.Execution.Domain.Repositories;

namespace KinemoPlatform.API.Execution.Application.Internal.QueryServices;

/// <summary>
///     Implementation of the show execution query service.
/// </summary>
public class ShowExecutionQueryService(IExecutionRepository executionRepository) : IShowExecutionQueryService
{
    public Task<IEnumerable<ShowExecution>> Handle(GetAllExecutionsQuery query, CancellationToken cancellationToken = default) =>
        executionRepository.ListWithDetailsAsync(cancellationToken);

    public async Task<ShowExecution?> Handle(GetExecutionByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await executionRepository.FindByIdWithDetailsAsync(query.ExecutionId, cancellationToken);
    }

    public async Task<ShowExecution?> Handle(GetExecutionByShowIdQuery query, CancellationToken cancellationToken = default)
    {
        return await executionRepository.FindActiveByShowIdAsync(query.ShowId, cancellationToken);
    }
}