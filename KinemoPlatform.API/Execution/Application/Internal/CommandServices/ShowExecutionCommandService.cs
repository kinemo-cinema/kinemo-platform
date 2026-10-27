using Cortex.Mediator;
using KinemoPlatform.API.Execution.Domain.Model.Aggregates;
using KinemoPlatform.API.Execution.Domain.Model.Commands;
using KinemoPlatform.API.Execution.Domain.Model.Events;
using KinemoPlatform.API.Execution.Domain.Repositories;
using KinemoPlatform.API.Execution.Domain.Services;
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
    public async Task<ShowExecution?> Handle(StartExecutionCommand command)
    {
        // Check if there is already an active execution for this show
        var existingExecution = await executionRepository.FindActiveByShowIdAsync(command.ShowId);
        if (existingExecution is not null)
            throw new InvalidOperationException($"An active execution already exists for Show {command.ShowId}.");

        var execution = new ShowExecution(command);
        await executionRepository.AddAsync(execution);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new ExecutionStartedEvent(execution.Id, execution.ShowId));

        return execution;
    }

    public async Task<ShowExecution?> Handle(PauseExecutionCommand command)
    {
        var execution = await executionRepository.FindByIdWithDetailsAsync(command.ExecutionId);
        if (execution is null) throw new InvalidOperationException($"Execution {command.ExecutionId} not found.");

        execution.Pause();
        executionRepository.Update(execution);
        await unitOfWork.CompleteAsync();

        return execution;
    }

    public async Task<ShowExecution?> Handle(ResumeExecutionCommand command)
    {
        var execution = await executionRepository.FindByIdWithDetailsAsync(command.ExecutionId);
        if (execution is null) throw new InvalidOperationException($"Execution {command.ExecutionId} not found.");

        execution.Resume();
        executionRepository.Update(execution);
        await unitOfWork.CompleteAsync();

        return execution;
    }

    public async Task<ShowExecution?> Handle(FinishExecutionCommand command)
    {
        var execution = await executionRepository.FindByIdWithDetailsAsync(command.ExecutionId);
        if (execution is null) throw new InvalidOperationException($"Execution {command.ExecutionId} not found.");

        execution.Finish();
        executionRepository.Update(execution);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(new ExecutionFinishedEvent(execution.Id));

        return execution;
    }

    public async Task<ShowExecution?> Handle(TriggerEmergencyStopCommand command)
    {
        var execution = await executionRepository.FindByIdWithDetailsAsync(command.ExecutionId);
        if (execution is null) throw new InvalidOperationException($"Execution {command.ExecutionId} not found.");

        execution.TriggerEmergencyStop(command.Reason);
        executionRepository.Update(execution);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new EmergencyStopTriggeredEvent(execution.Id, command.Reason));

        return execution;
    }
}