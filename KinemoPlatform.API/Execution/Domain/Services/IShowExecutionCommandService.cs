using KinemoPlatform.API.Execution.Domain.Model.Aggregates;
using KinemoPlatform.API.Execution.Domain.Model.Commands;

namespace KinemoPlatform.API.Execution.Domain.Services;

/// <summary>
///     Represents the show execution command service in the Kinemo Platform.
/// </summary>
public interface IShowExecutionCommandService
{
    /// <summary>
    ///     Handles starting a new execution (US13).
    /// </summary>
    Task<ShowExecution?> Handle(StartExecutionCommand command);

    /// <summary>
    ///     Handles pausing an active execution (US16).
    /// </summary>
    Task<ShowExecution?> Handle(PauseExecutionCommand command);

    /// <summary>
    ///     Handles resuming a paused execution (US16).
    /// </summary>
    Task<ShowExecution?> Handle(ResumeExecutionCommand command);

    /// <summary>
    ///     Handles finishing an execution normally (US15).
    /// </summary>
    Task<ShowExecution?> Handle(FinishExecutionCommand command);

    /// <summary>
    ///     Handles triggering an emergency stop (US14).
    /// </summary>
    Task<ShowExecution?> Handle(TriggerEmergencyStopCommand command);
}