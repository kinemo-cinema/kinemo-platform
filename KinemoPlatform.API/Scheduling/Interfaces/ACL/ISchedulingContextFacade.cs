namespace KinemoPlatform.API.Scheduling.Interfaces.ACL;

/// <summary>A show's room and time window, as seen by other bounded contexts.</summary>
public record ShowSlot(int ShowId, int RoomId, DateTimeOffset StartDateTime, DateTimeOffset EndDateTime);

public record ShowInfo(int ShowId, int RoomId, bool IsScheduled);


/// <summary>Facade that exposes Scheduling data to other bounded contexts.</summary>
public interface ISchedulingContextFacade
{
    /// <summary>Shows that are not cancelled and overlap the given period.</summary>
    Task<IReadOnlyList<ShowSlot>> FetchActiveShowSlots(
        DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default);
    
    /// <summary>Basic info of a show, or null when it does not exist.</summary>
    Task<ShowInfo?> FetchShowInfo(int showId, CancellationToken cancellationToken = default);
}