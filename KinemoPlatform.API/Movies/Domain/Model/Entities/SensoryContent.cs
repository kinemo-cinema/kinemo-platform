using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Domain.Model.Entities;

/// <summary>
///     Represents the sensory configuration associated with a movie (1-to-1).
/// </summary>
/// <remarks>
///     This entity belongs to the Movie aggregate and is not accessible
///     independently. It groups the sensory tracks and the configuration history.
/// </remarks>
public class SensoryContent
{
    /// <summary>
    ///     Default constructor for the sensory content entity.
    /// </summary>
    public SensoryContent()
    {
        IntensityLevel = ESensoryIntensityLevel.Medium;
        Status = ESensoryContentStatus.Draft;
        Tracks = new List<SensoryTrack>();
        History = new List<ConfigurationHistoryEntry>();
    }

    public int Id { get; }
    public int MovieId { get; private set; }
    public ESensoryIntensityLevel IntensityLevel { get; private set; }
    public ESensoryContentStatus Status { get; private set; }

    public ICollection<SensoryTrack> Tracks { get; }
    public ICollection<ConfigurationHistoryEntry> History { get; }

    /// <summary>
    ///     Add a sensory track to this configuration.
    /// </summary>
    /// <param name="effectType">The effect channel of the track.</param>
    /// <param name="filePath">The storage path of the sensory file.</param>
    /// <param name="durationSeconds">The duration of the track in seconds.</param>
    public void AddTrack(EEffectType effectType, string filePath, int durationSeconds)
    {
        // Prevent duplicate tracks per effect type
        if (Tracks.Any(t => t.EffectType == effectType)) return;
        Tracks.Add(new SensoryTrack(effectType, filePath, durationSeconds));
    }

    /// <summary>
    ///     Change the intensity level of the sensory configuration and
    ///     record the change in the configuration history.
    /// </summary>
    /// <param name="newIntensity">The new intensity level to apply.</param>
    public void ChangeIntensity(ESensoryIntensityLevel newIntensity)
    {
        if (IntensityLevel == newIntensity) return;
        var previous = IntensityLevel;
        IntensityLevel = newIntensity;
        History.Add(new ConfigurationHistoryEntry(previous, newIntensity));
    }

    /// <summary>
    ///     Mark the sensory content as ready for activation.
    /// </summary>
    public void MarkAsReady()
    {
        Status = ESensoryContentStatus.Ready;
    }

    /// <summary>
    ///     Activate the sensory content.
    /// </summary>
    public void Activate()
    {
        Status = ESensoryContentStatus.Active;
    }

    /// <summary>
    ///     Deactivate the sensory content.
    /// </summary>
    public void Deactivate()
    {
        Status = ESensoryContentStatus.Inactive;
    }
}