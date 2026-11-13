namespace KinemoPlatform.API.Rooms.Application.Errors;

/// <summary>
///     Expected failures of the Rooms command service. The REST layer maps each value to an HTTP response.
/// </summary>
public enum ERoomError
{
    /// <summary>
    ///     The room does not exist (HTTP 404).
    /// </summary>
    RoomNotFound,

    /// <summary>
    ///     Another room already uses the requested name (HTTP 409).
    /// </summary>
    DuplicateName,

    /// <summary>
    ///     The room is blocked or out of service, so it cannot be prepared until it is released (HTTP 409).
    /// </summary>
    RoomNotAvailableForPreparation,

    /// <summary>
    ///     The end of the blocking period is not after its start (HTTP 400).
    /// </summary>
    InvalidBlockPeriod
}