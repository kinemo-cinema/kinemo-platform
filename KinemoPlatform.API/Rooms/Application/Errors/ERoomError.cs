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
    DuplicateName
}