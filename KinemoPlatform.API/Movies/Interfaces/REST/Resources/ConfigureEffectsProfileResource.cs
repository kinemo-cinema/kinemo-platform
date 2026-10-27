using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Resources;

/// <summary>
///     Resource to configure the effects profile of a movie (US06).
/// </summary>
/// <param name="IntensityLevel">
///     The new <see cref="ESensoryIntensityLevel" /> to apply to the
///     sensory content of the movie.
/// </param>
public record ConfigureEffectsProfileResource(ESensoryIntensityLevel IntensityLevel);