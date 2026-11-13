using KinemoPlatform.API.Execution.Application.Errors;
using KinemoPlatform.API.Execution.Domain.Model.Aggregates;
using KinemoPlatform.API.Execution.Domain.Model.Commands;
using KinemoPlatform.API.Shared.Application.Patterns;

namespace KinemoPlatform.API.Execution.Application.Services;

/// <summary>
///     Represents the show execution command service in the Kinemo Platform.
/// </summary>
public interface IShowExecutionCommandService
{
    /// <summary>
    ///     Handles starting a new execution (US13).
    /// </summary>
    Task<Result<ShowExecution, EExecutionError>> Handle(StartExecutionCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles pausing an active execution (US16).
    /// </summary>
    Task<Result<ShowExecution, EExecutionError>> Handle(PauseExecutionCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles resuming a paused execution (US16).
    /// </summary>
    Task<Result<ShowExecution, EExecutionError>> Handle(ResumeExecutionCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles finishing an execution normally (US15).
    /// </summary>
    Task<Result<ShowExecution, EExecutionError>> Handle(FinishExecutionCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles triggering an emergency stop (US14).
    /// </summary>
    Task<Result<ShowExecution, EExecutionError>> Handle(TriggerEmergencyStopCommand command,
        CancellationToken cancellationToken = default);
}