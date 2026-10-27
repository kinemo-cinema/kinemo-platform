using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Domain.Model.Entities;

/// <summary>
///     Represents a sensory track belonging to a movie's sensory content.
/// </summary>
/// <remarks>
///     A track holds the file path of the effects timeline associated with a
///     specific physical effect channel (motion, wind, water or vibration).
/// </remarks>
public class SensoryTrack
{
    /// <summary>
    ///     Default constructor for the sensory track entity.
    /// </summary>
    public SensoryTrack()
    {
        FilePath = string.Empty;
        EffectType = EEffectType.Motion;
    }

    /// <summary>
    ///     Constructor for the sensory track entity.
    /// </summary>
    /// <param name="effectType">The effect channel of the track.</param>
    /// <param name="filePath">The storage path of the sensory file.</param>
    /// <param name="durationSeconds">The duration of the track in seconds.</param>
    public SensoryTrack(EEffectType effectType, string filePath, int durationSeconds) : this()
    {
        EffectType = effectType;
        FilePath = filePath;
        DurationSeconds = durationSeconds;
    }

    public int Id { get; }
    public int SensoryContentId { get; private set; }
    public EEffectType EffectType { get; private set; }
    public string FilePath { get; private set; }
    public int DurationSeconds { get; private set; }
}