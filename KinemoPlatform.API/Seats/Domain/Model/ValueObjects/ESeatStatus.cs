namespace KinemoPlatform.API.Seats.Domain.Model.ValueObjects;

/// <summary>
///     Represents the operational status of a seat for a specific show.
/// </summary>
public enum ESeatStatus
{
    Available,
    Sold,
    Enabled,
    Disabled,
    OutOfService
}