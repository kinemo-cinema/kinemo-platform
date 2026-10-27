using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Resources;

/// <summary>
///     Sensory track resource for REST API responses.
/// </summary>
/// <param name="Id">
///     The unique identifier of the sensory track.
/// </param>
/// <param name="EffectType">
///     The <see cref="EEffectType" /> channel of the track.
/// </param>
/// <param name="FilePath">
///     The storage path where the sensory file is stored.
/// </param>
/// <param name="DurationSeconds">
///     The duration of the sensory track in seconds.
/// </param>
public record SensoryTrackResource(
    int Id,
    EEffectType EffectType,
    string FilePath,
    int DurationSeconds);