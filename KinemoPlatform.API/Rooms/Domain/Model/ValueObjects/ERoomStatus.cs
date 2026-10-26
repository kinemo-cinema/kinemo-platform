namespace KinemoPlatform.API.Rooms.Domain.Model.ValueObjects;

/// <summary>
///     Represents the operational status of a 4D room.
/// </summary>
public enum ERoomStatus
{
    Available,
    Preparing,
    Maintenance,
    OutOfService
}