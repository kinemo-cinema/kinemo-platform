using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Domain.Model.Commands;

/// <summary>
///     Command to configure the effects profile of a movie by changing
///     the sensory intensity level applied to all effect channels.
/// </summary>
/// <param name="MovieId">
///     The unique identifier of the target movie.
/// </param>
/// <param name="IntensityLevel">
///     The new <see cref="ESensoryIntensityLevel" /> to apply.
/// </param>
public record ConfigureEffectsProfileCommand(int MovieId, ESensoryIntensityLevel IntensityLevel);