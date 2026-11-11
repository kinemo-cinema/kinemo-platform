namespace KinemoPlatform.API.Movies.Application.Errors;

/// <summary>
///     Expected failures of the Movies command service. The REST layer maps each value to an HTTP response.
/// </summary>
public enum EMovieError
{
    /// <summary>
    ///     The movie does not exist (HTTP 404).
    /// </summary>
    MovieNotFound,

    /// <summary>
    ///     Another movie already uses the requested title (HTTP 409).
    /// </summary>
    DuplicateTitle,

    /// <summary>
    ///     The movie already has a sensory track for the requested effect type (HTTP 409).
    /// </summary>
    DuplicateEffectType
}