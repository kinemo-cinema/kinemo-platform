using KinemoPlatform.API.Execution.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Execution.Domain.Repositories;

/// <summary>
///     Represents the show execution repository in the Kinemo Platform.
/// </summary>
public interface IExecutionRepository : IBaseRepository<ShowExecution>
{
    /// <summary>
    ///     Retrieves an execution by its ID, eagerly loading all child entities
    ///     (sequences, logs, emergency events).
    /// </summary>
    /// <param name="executionId">The unique identifier of the execution.</param>
    /// <returns>The execution with its related entities, or null if not found.</returns>
    Task<ShowExecution?> FindByIdWithDetailsAsync(int executionId);

    /// <summary>
    ///     Retrieves the active execution for a specific show.
    /// </summary>
    /// <param name="showId">The ID of the show.</param>
    /// <returns>The execution if found, or null.</returns>
    Task<ShowExecution?> FindActiveByShowIdAsync(int showId);
}