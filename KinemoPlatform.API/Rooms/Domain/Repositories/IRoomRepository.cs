using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Rooms.Domain.Repositories;

/// <summary>
///     Represents the room repository in the Kinemo Platform.
/// </summary>
public interface IRoomRepository : IBaseRepository<Room>
{
    /// <summary>
    ///     Checks if a room with the given name already exists.
    /// </summary>
    /// <param name="name">The name of the room.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if it exists, false otherwise.</returns>
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a room by its ID, eagerly loading its inspections and blocks.
    /// </summary>
    /// <param name="roomId">The unique identifier of the room.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The room with its related entities, or null if not found.</returns>
    Task<Room?> FindByIdWithDetailsAsync(int roomId, CancellationToken cancellationToken = default);
}