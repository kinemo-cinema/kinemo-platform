namespace KinemoPlatform.API.Execution.Domain.Model.Entities;

/// <summary>
///     Logs raw messages received from the physical hardware during execution.
/// </summary>
public class HardwareExecutionLog
{
    public HardwareExecutionLog()
    {
        Message = string.Empty;
    }

    public HardwareExecutionLog(string message, string severity) : this()
    {
        Message = message;
        Severity = severity;
        OccurredAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; }
    public int ExecutionId { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }
    public string Message { get; private set; }
    public string Severity { get; private set; } = string.Empty;
}