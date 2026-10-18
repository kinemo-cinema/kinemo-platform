using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Domain.Services;

/// <summary>
///     Represents the movie sensory domain service in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Encapsulates pure domain rules that span more than one entity or
///     require business validation beyond simple state mutation.
///     Implementations of this interface must not depend on repositories.
/// </remarks>
public interface IMovieSensoryDomainService
{
    /// <summary>
    ///     Verify whether the given intensity level is within the allowed
    ///     range for a sensory content configuration.
    /// </summary>
    /// <param name="intensity">
    ///     The <see cref="ESensoryIntensityLevel" /> to validate.
    /// </param>
    /// <returns>
    ///     True if the intensity is valid, false otherwise.
    /// </returns>
    bool IsValidIntensity(ESensoryIntensityLevel intensity);

    /// <summary>
    ///     Verify whether a sensory track of the given effect type can be
    ///     associated based on the room's physical capabilities.
    /// </summary>
    /// <param name="effectType">
    ///     The <see cref="EEffectType" /> channel to validate.
    /// </param>
    /// <returns>
    ///     True if the effect type is supported, false otherwise.
    /// </returns>
    bool IsSupportedEffectType(EEffectType effectType);

    /// <summary>
    ///     Verify whether a movie can be deactivated based on its current
    ///     sensory content status.
    /// </summary>
    /// <param name="currentStatus">
    ///     The current <see cref="ESensoryContentStatus" /> of the movie.
    /// </param>
    /// <returns>
    ///     True if the movie can be deactivated, false otherwise.
    /// </returns>
    bool CanDeactivate(ESensoryContentStatus currentStatus);
}