namespace KinemoPlatform.API.Scheduling.Application.Errors;

/// <summary>
///     Expected failures of the Shows command service. The REST layer maps each value to an HTTP response.
/// </summary>
public enum EShowError
{
    /// <summary>
    ///     The show does not exist (HTTP 404).
    /// </summary>
    ShowNotFound,

    /// <summary>
    ///     The room is already booked for the requested time slot when scheduling a show (HTTP 409).
    /// </summary>
    RoomAlreadyBooked,

    /// <summary>
    ///     The new time slot overlaps with another show in the same room when editing a show (HTTP 409).
    /// </summary>
    TimeSlotOverlap,

    /// <summary>
    ///     The end of the time slot is not after its start (HTTP 400).
    /// </summary>
    InvalidTimeSlot
}