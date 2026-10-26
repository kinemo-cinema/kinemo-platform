namespace KinemoPlatform.API.Rooms.Domain.Model.Entities;

/// <summary>
///     Represents a period of time during which a room is unavailable.
/// </summary>
public class RoomBlock
{
    public RoomBlock()
    {
        Reason = string.Empty;
    }

    public RoomBlock(string reason, DateTimeOffset startDateTime, DateTimeOffset endDateTime) : this()
    {
        Reason = reason;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        Active = true;
    }

    public int Id { get; }
    public int RoomId { get; private set; }
    public string Reason { get; private set; }
    public DateTimeOffset StartDateTime { get; private set; }
    public DateTimeOffset EndDateTime { get; private set; }
    public bool Active { get; private set; }

    /// <summary>
    ///     Deactivates the block, making the room available again.
    /// </summary>
    public void Deactivate()
    {
        Active = false;
    }
}