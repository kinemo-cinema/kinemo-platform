namespace KinemoPlatform.API.Seats.Application.Errors;

/// <summary>
///     Expected failures of the Seat Allocations command service. The REST layer maps each value to an HTTP response.
/// </summary>
public enum ESeatAllocationError
{
    /// <summary>
    ///     A seat allocation already exists for the show (HTTP 409).
    /// </summary>
    AllocationAlreadyExists,

    /// <summary>
    ///     The show has no seat allocation (HTTP 404).
    /// </summary>
    AllocationNotFound,

    /// <summary>
    ///     The allocation has no seat with the requested number (HTTP 404).
    /// </summary>
    SeatNotFound,

    /// <summary>
    ///     The requested status cannot be assigned manually (HTTP 400).
    /// </summary>
    UnsupportedSeatStatus
}
