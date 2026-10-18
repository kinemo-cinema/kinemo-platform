using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Resources;

/// <summary>
///     Configuration history entry resource for REST API responses.
/// </summary>
/// <param name="Id">
///     The unique identifier of the history entry.
/// </param>
/// <param name="PreviousIntensity">
///     The intensity before the change.
/// </param>
/// <param name="NewIntensity">
///     The intensity after the change.
/// </param>
/// <param name="ChangedAt">
///     The moment when the change was applied.
/// </param>
public record ConfigurationHistoryResource(
    int Id,
    ESensoryIntensityLevel PreviousIntensity,
    ESensoryIntensityLevel NewIntensity,
    DateTimeOffset ChangedAt);