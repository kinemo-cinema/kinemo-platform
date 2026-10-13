using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;
using KinemoPlatform.API.Movies.Domain.Services;

namespace KinemoPlatform.API.Movies.Application.Internal.DomainServices;

/// <summary>
///     Implementation of the movie sensory domain service in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Contains pure domain logic that does not depend on repositories,
///     unit of work, or any infrastructure concern.
/// </remarks>
public class MovieSensoryDomainService : IMovieSensoryDomainService
{
    /// <inheritdoc />
    public bool IsValidIntensity(ESensoryIntensityLevel intensity)
    {
        return Enum.IsDefined(typeof(ESensoryIntensityLevel), intensity);
    }

    /// <inheritdoc />
    public bool IsSupportedEffectType(EEffectType effectType)
    {
        return Enum.IsDefined(typeof(EEffectType), effectType);
    }

    /// <inheritdoc />
    /// <remarks>
    ///     A movie cannot be deactivated while its sensory content is
    ///     in an <see cref="ESensoryContentStatus.Active" /> state, because
    ///     that would leave scheduled shows without a valid configuration.
    /// </remarks>
    public bool CanDeactivate(ESensoryContentStatus currentStatus)
    {
        return currentStatus != ESensoryContentStatus.Active;
    }
}