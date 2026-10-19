using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Scheduling.Domain.Repositories;

/// <summary>
///     Represents the show repository in the Kinemo Platform.
/// </summary>
public interface IShowRepository : IBaseRepository<Show>
{
    /// <summary>
    ///     Checks if there is any scheduled show in the given room that overlaps with the specified time slot.
    /// </summary>
    /// <param name="roomId">The ID of the room.</param>
    /// <param name="startDateTime">The start time of the new show.</param>
    /// <param name="endDateTime">The end time of the new show.</param>
    /// <returns>True if an overlapping show exists, false otherwise.</returns>
    Task<bool> ExistsOverlappingShowAsync(int roomId, DateTimeOffset startDateTime, DateTimeOffset endDateTime);

    /// <summary>
    ///     Retrieves all shows scheduled for a specific date.
    /// </summary>
    /// <param name="date">The date to filter by.</param>
    /// <returns>A collection of shows on that date.</returns>
    Task<IEnumerable<Show>> FindByDateAsync(DateTimeOffset date);
}