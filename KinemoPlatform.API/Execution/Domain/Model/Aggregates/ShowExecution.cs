using KinemoPlatform.API.Execution.Domain.Model.Commands;
using KinemoPlatform.API.Execution.Domain.Model.Entities;
using KinemoPlatform.API.Execution.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Execution.Domain.Model.Aggregates;

/// <summary>
///     Show Execution aggregate root.
/// </summary>
/// <remarks>
///     Manages the live execution of a 4D show, orchestrating sensory sequences,
///     hardware logs, and emergency stops.
/// </remarks>
public partial class ShowExecution
{
    public ShowExecution()
    {
        Status = EExecutionStatus.InProgress;
        StartedAt = DateTimeOffset.UtcNow;
        Sequences = new List<SensorySequence>();
        SyncEvents = new List<SynchronizationEvent>();
        HardwareLogs = new List<HardwareExecutionLog>();
        EmergencyEvents = new List<EmergencyEvent>();
    }

    public ShowExecution(StartExecutionCommand command) : this()
    {
        ShowId = command.ShowId;
        SensoryContentId = command.SensoryContentId;
    }

    public int Id { get; }
    public int ShowId { get; private set; }
    public int SensoryContentId { get; private set; }
    public EExecutionStatus Status { get; private set; }
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? FinishedAt { get; private set; }

    public ICollection<SensorySequence> Sequences { get; }
    public ICollection<SynchronizationEvent> SyncEvents { get; }
    public ICollection<HardwareExecutionLog> HardwareLogs { get; }
    public ICollection<EmergencyEvent> EmergencyEvents { get; }

    public void Pause()
    {
        if (Status != EExecutionStatus.InProgress) 
            throw new InvalidOperationException("Only an in-progress execution can be paused.");
        Status = EExecutionStatus.Paused;
    }

    public void Resume()
    {
        if (Status != EExecutionStatus.Paused) 
            throw new InvalidOperationException("Only a paused execution can be resumed.");
        Status = EExecutionStatus.InProgress;
    }

    public void Finish()
    {
        if (Status == EExecutionStatus.Finished || Status == EExecutionStatus.EmergencyStopped) 
            throw new InvalidOperationException("Execution is already finished or stopped.");
        
        Status = EExecutionStatus.Finished;
        FinishedAt = DateTimeOffset.UtcNow;
    }

    public void TriggerEmergencyStop(string reason)
    {
        Status = EExecutionStatus.EmergencyStopped;
        EmergencyEvents.Add(new EmergencyEvent(reason));
    }

    public void AddHardwareLog(string message, string severity)
    {
        HardwareLogs.Add(new HardwareExecutionLog(message, severity));
    }
}