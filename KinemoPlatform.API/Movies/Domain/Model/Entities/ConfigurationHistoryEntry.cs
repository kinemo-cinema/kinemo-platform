using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Domain.Model.Entities;

/// <summary>
///     Represents a single entry in the sensory configuration history of a movie.
/// </summary>
/// <remarks>
///     History entries are append-only: they record the previous and new
///     intensity levels whenever the effects profile of a movie is changed.
/// </remarks>
public class ConfigurationHistoryEntry
{
    /// <summary>
    ///     Default constructor for the configuration history entry.
    /// </summary>
    public ConfigurationHistoryEntry()
    {
        PreviousIntensity = ESensoryIntensityLevel.Medium;
        NewIntensity = ESensoryIntensityLevel.Medium;
        ChangedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Constructor for the configuration history entry.
    /// </summary>
    /// <param name="previousIntensity">The intensity before the change.</param>
    /// <param name="newIntensity">The intensity after the change.</param>
    public ConfigurationHistoryEntry(
        ESensoryIntensityLevel previousIntensity,
        ESensoryIntensityLevel newIntensity) : this()
    {
        PreviousIntensity = previousIntensity;
        NewIntensity = newIntensity;
    }

    public int Id { get; }
    public int SensoryContentId { get; private set; }
    public ESensoryIntensityLevel PreviousIntensity { get; private set; }
    public ESensoryIntensityLevel NewIntensity { get; private set; }
    public DateTimeOffset ChangedAt { get; private set; }
}