using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Domain.Model.Commands;

/// <summary>Stores an uploaded file and adds it as a track of the movie's sensory content.</summary>
public record UploadSensoryTrackCommand(
    int MovieId,
    EEffectType EffectType,
    int DurationSeconds,
    string FileName,
    Stream Content);