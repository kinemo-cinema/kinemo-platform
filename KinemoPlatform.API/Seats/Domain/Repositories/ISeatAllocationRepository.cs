using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Seats.Domain.Repositories;

/// <summary>
///     Represents the seat allocation repository in the Kinemo Platform.
/// </summary>
public interface ISeatAllocationRepository : IBaseRepository<SeatAllocation>
{
    /// <summary>
    ///     Checks if a seat allocation already exists for a specific show.
    /// </summary>
    /// <param name="showId">The ID of the show.</param>
    /// <returns>True if an allocation exists, false otherwise.</returns>
    Task<bool> ExistsByShowIdAsync(int showId);

    /// <summary>
    ///     Retrieves the seat allocation for a show, eagerly loading all seats and interventions.
    /// </summary>
    /// <param name="showId">The ID of the show.</param>
    /// <returns>The seat allocation with its related entities, or null if not found.</returns>
    Task<SeatAllocation?> FindByShowIdWithDetailsAsync(int showId);
}