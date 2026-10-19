using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Scheduling.Domain.Repositories;

public interface IShowRepository : IBaseRepository<Show>
{
    /// <summary>
    ///     Checks if there is any scheduled show in the given room that overlaps with the specified time slot.
    /// </summary>
    /// <param name="roomId">The ID of the room.</param>
    /// <param name="startDateTime">The start time of the new show.</param>
    /// <param name="endDateTime">The end time of the new show.</param>
    /// <param name="excludeShowId">Optional show ID to exclude from the check (used when editing an existing show).</param>
    /// <returns>True if an overlapping show exists, false otherwise.</returns>
    Task<bool> ExistsOverlappingShowAsync(int roomId, DateTimeOffset startDateTime, DateTimeOffset endDateTime, int? excludeShowId = null);

    Task<IEnumerable<Show>> FindByDateAsync(DateTimeOffset date);
}