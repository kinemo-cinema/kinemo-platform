using KinemoPlatform.API.Execution.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Execution.Domain.Model.Entities;

/// <summary>
///     Represents an emergency stop triggered during a show.
/// </summary>
public class EmergencyEvent
{
    public EmergencyEvent()
    {
        Reason = string.Empty;
    }

    public EmergencyEvent(string reason) : this()
    {
        Reason = reason;
        TriggeredAt = DateTimeOffset.UtcNow;
        Status = EEmergencyStatus.Unresolved;
    }

    public int Id { get; }
    public int ExecutionId { get; private set; }
    public string Reason { get; private set; }
    public DateTimeOffset TriggeredAt { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    public EEmergencyStatus Status { get; private set; }

    public void Resolve()
    {
        Status = EEmergencyStatus.Resolved;
        ResolvedAt = DateTimeOffset.UtcNow;
    }
}