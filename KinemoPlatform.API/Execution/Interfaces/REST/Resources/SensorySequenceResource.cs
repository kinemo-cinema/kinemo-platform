using KinemoPlatform.API.Execution.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Resources;

public record SensorySequenceResource(
    int Id,
    int SensoryTrackId,
    long CurrentTimeCode,
    long ActualTimeCode,
    ESyncStatus Status);