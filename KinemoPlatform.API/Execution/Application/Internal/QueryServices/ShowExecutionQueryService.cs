using KinemoPlatform.API.Execution.Domain.Model.Aggregates;
using KinemoPlatform.API.Execution.Domain.Model.Queries;
using KinemoPlatform.API.Execution.Domain.Repositories;
using KinemoPlatform.API.Execution.Domain.Services;

namespace KinemoPlatform.API.Execution.Application.Internal.QueryServices;

/// <summary>
///     Implementation of the show execution query service.
/// </summary>
public class ShowExecutionQueryService(IExecutionRepository executionRepository) : IShowExecutionQueryService
{
    public async Task<ShowExecution?> Handle(GetExecutionByIdQuery query)
    {
        return await executionRepository.FindByIdWithDetailsAsync(query.ExecutionId);
    }

    public async Task<ShowExecution?> Handle(GetExecutionByShowIdQuery query)
    {
        return await executionRepository.FindActiveByShowIdAsync(query.ShowId);
    }
}