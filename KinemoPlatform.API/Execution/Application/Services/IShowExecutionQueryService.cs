using KinemoPlatform.API.Execution.Domain.Model.Aggregates;
using KinemoPlatform.API.Execution.Domain.Model.Queries;

namespace KinemoPlatform.API.Execution.Application.Services;

/// <summary>
///     Represents the show execution query service in the Kinemo Platform.
/// </summary>
public interface IShowExecutionQueryService
{
    Task<IEnumerable<ShowExecution>> Handle(GetAllExecutionsQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles retrieving an execution by its unique ID.
    /// </summary>
    Task<ShowExecution?> Handle(GetExecutionByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles retrieving the current (or most recent) execution for a specific show.
    /// </summary>
    Task<ShowExecution?> Handle(GetExecutionByShowIdQuery query, CancellationToken cancellationToken = default);
}