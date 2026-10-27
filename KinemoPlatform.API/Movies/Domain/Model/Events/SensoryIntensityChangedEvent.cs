using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;
using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Movies.Domain.Model.Events;

/// <summary>
///     Domain event published when the sensory intensity level of a movie
///     is changed (US06).
/// </summary>
/// <remarks>
///     Consumed by BC08 (Operational Analytics) to track configuration
///     changes over time.
/// </remarks>
/// <param name="MovieId">
///     The unique identifier of the target movie.
/// </param>
/// <param name="PreviousIntensity">
///     The intensity before the change.
/// </param>
/// <param name="NewIntensity">
///     The intensity after the change.
/// </param>
public record SensoryIntensityChangedEvent(
    int MovieId,
    ESensoryIntensityLevel PreviousIntensity,
    ESensoryIntensityLevel NewIntensity) : IEvent;