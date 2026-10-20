using KinemoPlatform.API.Rooms.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Rooms.Domain.Model.Entities;

/// <summary>
///     Represents a readiness inspection performed on a room.
/// </summary>
public class RoomInspection
{
    public RoomInspection()
    {
        Observations = string.Empty;
    }

    public RoomInspection(string observations, EInspectionResult result) : this()
    {
        Observations = observations;
        Result = result;
        InspectedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; }
    public int RoomId { get; private set; }
    public DateTimeOffset InspectedAt { get; private set; }
    public string Observations { get; private set; }
    public EInspectionResult Result { get; private set; }
}