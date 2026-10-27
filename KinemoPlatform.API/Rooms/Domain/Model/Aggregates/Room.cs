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
    ///     Prepares the room by adding an inspection record. 
    ///     If passed, the room becomes Available.
    /// </summary>
    public void Prepare(string observations)
    {
        var inspection = new RoomInspection(observations, EInspectionResult.Passed);
        Inspections.Add(inspection);
        Status = ERoomStatus.Available;
    }

    /// <summary>
    ///     Blocks the room for maintenance or other reasons.
    /// </summary>
    public void Block(string reason, DateTimeOffset startDateTime, DateTimeOffset endDateTime)
    {
        var block = new RoomBlock(reason, startDateTime, endDateTime);
        Blocks.Add(block);
        Status = ERoomStatus.Maintenance;
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