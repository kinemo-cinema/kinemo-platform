using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Resources;

/// <summary>
///     Sensory content resource for REST API responses.
/// </summary>
/// <param name="Id">
///     The unique identifier of the sensory content.
/// </param>
/// <param name="IntensityLevel">
///     The <see cref="ESensoryIntensityLevel" /> currently applied to the
///     sensory content.
/// </param>
/// <param name="Status">
///     The <see cref="ESensoryContentStatus" /> preparation status of the
///     sensory content.
/// </param>
/// <param name="Tracks">
///     The sensory tracks associated with this content.
/// </param>
/// <param name="History">
///     The configuration history of this content.
/// </param>
public record SensoryContentResource(
    int Id,
    ESensoryIntensityLevel IntensityLevel,
    ESensoryContentStatus Status,
    IEnumerable<SensoryTrackResource> Tracks,
    IEnumerable<ConfigurationHistoryResource> History);