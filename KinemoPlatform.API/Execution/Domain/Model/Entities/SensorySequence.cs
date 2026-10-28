using KinemoPlatform.API.Execution.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Execution.Domain.Model.Entities;

/// <summary>
///     Represents the live execution state of a specific sensory track (e.g., Wind, Motion).
/// </summary>
public class SensorySequence
{
    public SensorySequence()
    {
    }

    public SensorySequence(int sensoryTrackId) : this()
    {
        SensoryTrackId = sensoryTrackId;
        Status = ESyncStatus.Synchronized;
        CurrentTimeCode = 0;
        ActualTimeCode = 0;
    }

    public int Id { get; }
    public int ExecutionId { get; private set; }
    public int SensoryTrackId { get; private set; }
    public long CurrentTimeCode { get; private set; }
    public long ActualTimeCode { get; private set; }
    public ESyncStatus Status { get; private set; }

    public void UpdateTimeCodes(long expectedTimeCode, long actualTimeCode)
    {
        CurrentTimeCode = expectedTimeCode;
        ActualTimeCode = actualTimeCode;

        // Simple drift logic: if difference is > 500ms, consider it drifting
        var drift = Math.Abs(expectedTimeCode - actualTimeCode);
        Status = drift > 500 ? ESyncStatus.Drifting : ESyncStatus.Synchronized;
    }
}