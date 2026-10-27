namespace KinemoPlatform.API.Execution.Domain.Model.Entities;

/// <summary>
///     Records a synchronization drift event for analytics.
/// </summary>
public class SynchronizationEvent
{
    public SynchronizationEvent()
    {
    }

    public SynchronizationEvent(long expectedTimeCode, long actualTimeCode, string eventType) : this()
    {
        ExpectedTimeCode = expectedTimeCode;
        ActualTimeCode = actualTimeCode;
        DriftMilliseconds = expectedTimeCode - actualTimeCode;
        EventType = eventType;
        OccurredAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; }
    public int ExecutionId { get; private set; }
    public long ExpectedTimeCode { get; private set; }
    public long ActualTimeCode { get; private set; }
    public long DriftMilliseconds { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }
    public string EventType { get; private set; } = string.Empty;
}