namespace KinemoPlatform.API.Rooms.Interfaces.ACL;

public record RoomSlotAvailability(bool RoomExists, bool IsBlocked);

/// <summary>Facade that exposes Rooms data to other bounded contexts.</summary>
public interface IRoomsContextFacade
{
    /// <summary>Ids of all registered rooms.</summary>
    Task<IReadOnlyList<int>> FetchRoomIds(CancellationToken cancellationToken = default);
    
    /// <summary>Seat capacity of a room, or 0 when the room does not exist.</summary>
    Task<int> FetchRoomCapacity(int roomId, CancellationToken cancellationToken = default);
    
    /// <summary>Whether the room exists and has an active block overlapping [from, to).</summary>
    Task<RoomSlotAvailability> FetchRoomSlotAvailability(
        int roomId, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default);
}