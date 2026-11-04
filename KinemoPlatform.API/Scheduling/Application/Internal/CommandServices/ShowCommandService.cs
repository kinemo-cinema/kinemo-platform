using Cortex.Mediator;
using KinemoPlatform.API.Scheduling.Application.Errors;
using KinemoPlatform.API.Scheduling.Application.Services;
using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Scheduling.Domain.Model.Commands;
using KinemoPlatform.API.Scheduling.Domain.Model.Events;
using KinemoPlatform.API.Scheduling.Domain.Model.ValueObjects;
using KinemoPlatform.API.Scheduling.Domain.Repositories;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Scheduling.Application.Internal.CommandServices;

/// <summary>
///     Implementation of the show command service.
/// </summary>
/// <remarks>
///     Expected failures (show not found, invalid or conflicting time slot) are returned as
///     <see cref="EShowError" /> inside a <see cref="Result{TValue,TError}" />. Unexpected failures are not caught
///     here; they surface as exceptions and are turned into a ProblemDetails 500 response globally.
/// </remarks>
public class ShowCommandService(
    IShowRepository showRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IShowCommandService
{
    public async Task<Result<Show, EShowError>> Handle(ScheduleShowCommand command,
        CancellationToken cancellationToken = default)
    {
        // 0. The show must end after it starts
        if (!new TimeSlot(command.StartDateTime, command.EndDateTime).IsValid())
            return Fail(EShowError.InvalidTimeSlot);

        // 1. Check for room scheduling conflicts
        // (the token is passed by name because excludeShowId is the optional parameter before it)
        var hasConflict = await showRepository.ExistsOverlappingShowAsync(
            command.RoomId, command.StartDateTime, command.EndDateTime, cancellationToken: cancellationToken);

        if (hasConflict) return Fail(EShowError.RoomAlreadyBooked);

        // 2. Create and persist the show
        var show = new Show(command);
        await showRepository.AddAsync(show, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        // 3. Publish event
        await domainEventPublisher.PublishAsync(
            new ShowScheduledEvent(show.Id, show.MovieId, show.RoomId, show.TimeSlot.StartDateTime),
            cancellationToken);

        return Ok(show);
    }

    public async Task<Result<Show, EShowError>> Handle(EditShowCommand command,
        CancellationToken cancellationToken = default)
    {
        var show = await showRepository.FindByIdAsync(command.ShowId, cancellationToken);
        if (show is null) return Fail(EShowError.ShowNotFound);

        if (!new TimeSlot(command.StartDateTime, command.EndDateTime).IsValid())
            return Fail(EShowError.InvalidTimeSlot);

        // Check for conflicts, excluding the show currently being edited
        var hasConflict = await showRepository.ExistsOverlappingShowAsync(
            show.RoomId, command.StartDateTime, command.EndDateTime,
            excludeShowId: show.Id, cancellationToken: cancellationToken);

        if (hasConflict) return Fail(EShowError.TimeSlotOverlap);

        show.UpdateTimeSlot(command.StartDateTime, command.EndDateTime);
        showRepository.Update(show);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Ok(show);
    }

    public async Task<Result<Show, EShowError>> Handle(CancelShowCommand command,
        CancellationToken cancellationToken = default)
    {
        var show = await showRepository.FindByIdAsync(command.ShowId, cancellationToken);
        if (show is null) return Fail(EShowError.ShowNotFound);

        show.Cancel();
        showRepository.Update(show);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(new ShowCancelledEvent(show.Id), cancellationToken);

        return Ok(show);
    }

    private static Result<Show, EShowError> Ok(Show show) =>
        new Result<Show, EShowError>.Success(show);

    private static Result<Show, EShowError> Fail(EShowError error) =>
        new Result<Show, EShowError>.Failure(error);
}