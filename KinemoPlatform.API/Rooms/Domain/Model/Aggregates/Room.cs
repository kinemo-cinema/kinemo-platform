using KinemoPlatform.API.Rooms.Domain.Model.Commands;
using KinemoPlatform.API.Rooms.Domain.Model.Entities;
using KinemoPlatform.API.Rooms.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Rooms.Domain.Model.Aggregates;

/// <summary>
///     Room aggregate root entity in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Manages the physical room, its readiness inspections, and blocking periods.
/// </remarks>
public partial class Room
{
    public Room()
    {
        Name = string.Empty;
        Status = ERoomStatus.OutOfService; // Default until prepared
        Inspections = new List<RoomInspection>();
        Blocks = new List<RoomBlock>();
    }

    public Room(CreateRoomCommand command) : this()
    {
        Name = command.Name;
        Capacity = command.Capacity;
        Status = ERoomStatus.Available;
    }

    public int Id { get; }
    public string Name { get; private set; }
    public int Capacity { get; private set; }
    public ERoomStatus Status { get; private set; }

    public ICollection<RoomInspection> Inspections { get; }
    public ICollection<RoomBlock> Blocks { get; }

    /// <summary>
    ///     Whether the room can be prepared: it must not be in maintenance or out of service and it must
    ///     not have an active block. A blocked room has to be released first.
    /// </summary>
    public bool CanBePrepared()
    {
        return (Status is ERoomStatus.Available or ERoomStatus.Preparing) && !Blocks.Any(b => b.Active);
    }

    /// <summary>
    ///     Prepares the room by adding an inspection record. 
    ///     If passed, the room becomes Available.
    /// </summary>
    /// <returns>The inspection that was recorded.</returns>
    /// <exception cref="InvalidOperationException">The room is blocked or out of service.</exception>
    public RoomInspection Prepare(string observations)
    {
        if (!CanBePrepared())
            throw new InvalidOperationException("A blocked or out-of-service room cannot be prepared. Release it first.");

        var inspection = new RoomInspection(observations, EInspectionResult.Passed);
        Inspections.Add(inspection);
        Status = ERoomStatus.Available;
        return inspection;
    }

    /// <summary>
    ///     Blocks the room for maintenance or other reasons.
    /// </summary>
    /// <returns>The block that was recorded.</returns>
    /// <exception cref="ArgumentException">The end of the period is not after its start.</exception>
    public RoomBlock Block(string reason, DateTimeOffset startDateTime, DateTimeOffset endDateTime)
    {
        if (endDateTime <= startDateTime)
            throw new ArgumentException("The end of the block must be after its start.", nameof(endDateTime));

        var block = new RoomBlock(reason, startDateTime, endDateTime);
        Blocks.Add(block);
        Status = ERoomStatus.Maintenance;
        return block;
    }

    /// <summary>
    ///     Releases the room from maintenance, deactivating all active blocks.
    /// </summary>
    public void Release()
    {
        foreach (var block in Blocks.Where(b => b.Active))
        {
            block.Deactivate();
        }
        Status = ERoomStatus.Available;
    }
}