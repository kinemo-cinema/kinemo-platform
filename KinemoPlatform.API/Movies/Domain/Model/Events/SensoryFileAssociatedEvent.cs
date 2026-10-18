using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;
using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Movies.Domain.Model.Events;

/// <summary>
///     Domain event published when a sensory file is associated to a movie (US02).
/// </summary>
/// <remarks>
///     Consumed by BC07 (Resource Testing &amp; Maintenance) to trigger
///     a channel test on the newly associated effect.
/// </remarks>
/// <param name="MovieId">
///     The unique identifier of the target movie.
/// </param>
/// <param name="EffectType">
///     The effect channel of the associated sensory track.
/// </param>
/// <param name="FilePath">
///     The storage path where the sensory file was saved.
/// </param>
public record SensoryFileAssociatedEvent(int MovieId, EEffectType EffectType, string FilePath) : IEvent;