using Cortex.Mediator;
using KinemoPlatform.API.Execution.Application.Errors;
using KinemoPlatform.API.Execution.Application.Services;
using KinemoPlatform.API.Execution.Domain.Model.Aggregates;
using KinemoPlatform.API.Execution.Domain.Model.Commands;
using KinemoPlatform.API.Execution.Domain.Model.Events;
using KinemoPlatform.API.Execution.Domain.Repositories;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Execution.Application.Internal.CommandServices;

/// <summary>
///     Implementation of the show execution command service.
/// </summary>
public class ShowExecutionCommandService(
    IExecutionRepository executionRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IShowExecutionCommandService
{
    public async Task<Result<ShowExecution, EExecutionError>> Handle(StartExecutionCommand command,
        CancellationToken cancellationToken = default)
    {
        // Existence check only — no need to load the whole object graph.
        if (await executionRepository.ExistsActiveByShowIdAsync(command.ShowId, cancellationToken))
            return Fail(EExecutionError.ActiveExecutionExists);

        var execution = new ShowExecution(command);
        await executionRepository.AddAsync(execution, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new ExecutionStartedEvent(execution.Id, execution.ShowId), cancellationToken);

        return Ok(execution);
    }

    public async Task<Result<ShowExecution, EExecutionError>> Handle(PauseExecutionCommand command,
        CancellationToken cancellationToken = default)
    {
        var execution = await executionRepository.FindByIdWithDetailsAsync(command.ExecutionId, cancellationToken);
        if (execution is null) return Fail(EExecutionError.ExecutionNotFound);
        if (!execution.CanPause()) return Fail(EExecutionError.InvalidExecutionState);

        execution.Pause();
        executionRepository.Update(execution);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Ok(execution);
    }

    public async Task<Result<ShowExecution, EExecutionError>> Handle(ResumeExecutionCommand command,
        CancellationToken cancellationToken = default)
    {
        var execution = await executionRepository.FindByIdWithDetailsAsync(command.ExecutionId, cancellationToken);
        if (execution is null) return Fail(EExecutionError.ExecutionNotFound);
        if (!execution.CanResume()) return Fail(EExecutionError.InvalidExecutionState);

        execution.Resume();
        executionRepository.Update(execution);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Ok(execution);
    }

    public async Task<Result<ShowExecution, EExecutionError>> Handle(FinishExecutionCommand command,
        CancellationToken cancellationToken = default)
    {
        var execution = await executionRepository.FindByIdWithDetailsAsync(command.ExecutionId, cancellationToken);
        if (execution is null) return Fail(EExecutionError.ExecutionNotFound);
        if (!execution.CanFinish()) return Fail(EExecutionError.InvalidExecutionState);

        execution.Finish();
        executionRepository.Update(execution);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new ExecutionFinishedEvent(execution.Id), cancellationToken);

        return Ok(execution);
    }

    public async Task<Result<ShowExecution, EExecutionError>> Handle(TriggerEmergencyStopCommand command,
        CancellationToken cancellationToken = default)
    {
        var execution = await executionRepository.FindByIdWithDetailsAsync(command.ExecutionId, cancellationToken);
        if (execution is null) return Fail(EExecutionError.ExecutionNotFound);
        if (!execution.CanTriggerEmergencyStop()) return Fail(EExecutionError.InvalidExecutionState);

        execution.TriggerEmergencyStop(command.Reason);
        executionRepository.Update(execution);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new EmergencyStopTriggeredEvent(execution.Id, command.Reason), cancellationToken);

        return Ok(execution);
    }

    private static Result<ShowExecution, EExecutionError> Ok(ShowExecution e) =>
        new Result<ShowExecution, EExecutionError>.Success(e);

    private static Result<ShowExecution, EExecutionError> Fail(EExecutionError error) =>
        new Result<ShowExecution, EExecutionError>.Failure(error);
}