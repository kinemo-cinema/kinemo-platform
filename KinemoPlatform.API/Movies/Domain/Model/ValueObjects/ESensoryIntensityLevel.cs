namespace KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

/// <summary>
///     Represents the intensity level of a sensory content configuration.
/// </summary>
/// <remarks>
///     The intensity level is applied uniformly to all effect channels
///     during the execution of a 4D show.
/// </remarks>
public enum ESensoryIntensityLevel
{
    Low,
    Medium,
    High
}