namespace KinemoPlatform.API.Scheduling.Interfaces.ACL;

/// <summary>A show's room and time window, as seen by other bounded contexts.</summary>
public record ShowSlot(int ShowId, int RoomId, DateTimeOffset StartDateTime, DateTimeOffset EndDateTime);

/// <summary>Facade that exposes Scheduling data to other bounded contexts.</summary>
public interface ISchedulingContextFacade
{
    /// <summary>Shows that are not cancelled and overlap the given period.</summary>
    Task<IReadOnlyList<ShowSlot>> FetchActiveShowSlots(
        DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default);
}