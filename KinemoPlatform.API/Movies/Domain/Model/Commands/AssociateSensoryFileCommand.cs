using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Domain.Model.Commands;

/// <summary>
///     Command to associate a sensory file to a movie's sensory content.
/// </summary>
/// <param name="MovieId">
///     The unique identifier of the target movie.
/// </param>
/// <param name="EffectType">
///     The <see cref="EEffectType" /> channel of the sensory track.
/// </param>
/// <param name="FilePath">
///     The storage path where the sensory file was saved.
/// </param>
/// <param name="DurationSeconds">
///     The duration of the sensory track in seconds.
/// </param>
public record AssociateSensoryFileCommand(int MovieId, EEffectType EffectType, string FilePath, int DurationSeconds);