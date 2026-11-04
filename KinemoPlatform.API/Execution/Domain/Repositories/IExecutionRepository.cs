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
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The execution with its related entities, or null if not found.</returns>
    Task<ShowExecution?> FindByIdWithDetailsAsync(int executionId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves the active execution for a specific show.
    /// </summary>
    /// <param name="showId">The ID of the show.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The execution if found, or null.</returns>
    Task<ShowExecution?> FindActiveByShowIdAsync(int showId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Checks whether an active execution exists for a specific show without
    ///     loading the full object graph.
    /// </summary>
    /// <param name="showId">The ID of the show.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns><c>true</c> if an active execution exists; otherwise, <c>false</c>.</returns>
    Task<bool> ExistsActiveByShowIdAsync(int showId, CancellationToken cancellationToken = default);
}