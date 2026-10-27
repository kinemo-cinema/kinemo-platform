namespace KinemoPlatform.API.Execution.Domain.Model.ValueObjects;

/// <summary>
///     Represents the live status of a show's execution.
/// </summary>
public enum EExecutionStatus
{
    InProgress,
    Paused,
    Finished,
    EmergencyStopped
}