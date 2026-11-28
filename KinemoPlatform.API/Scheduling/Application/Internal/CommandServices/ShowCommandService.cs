using Cortex.Mediator;
using KinemoPlatform.API.Movies.Interfaces.ACL;
using KinemoPlatform.API.Rooms.Interfaces.ACL;
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

public class ShowCommandService(
    IShowRepository showRepository,
    IMoviesContextFacade moviesFacade,
    IRoomsContextFacade roomsFacade,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IShowCommandService
{
    public async Task<Result<Show, EShowError>> Handle(ScheduleShowCommand command,
        CancellationToken cancellationToken = default)
    {
        // 0. The show must end after it starts
        if (!new TimeSlot(command.StartDateTime, command.EndDateTime).IsValid())
            return Fail(EShowError.InvalidTimeSlot);

        // 1. The movie must exist and be active (US04, US07)
        var movie = await moviesFacade.FetchMovieInfo(command.MovieId, cancellationToken);
        if (movie is null) return Fail(EShowError.MovieNotFound);
        if (!movie.IsActive) return Fail(EShowError.MovieNotActive);

        // 2. The room must exist and not be blocked during the slot (US25)
        var slot = await roomsFacade.FetchRoomSlotAvailability(
            command.RoomId, command.StartDateTime, command.EndDateTime, cancellationToken);
        if (!slot.RoomExists) return Fail(EShowError.RoomNotFound);
        if (slot.IsBlocked) return Fail(EShowError.RoomBlocked);

        // 3. Check for room scheduling conflicts
        // (the token is passed by name because excludeShowId is the optional parameter before it)
        var hasConflict = await showRepository.ExistsOverlappingShowAsync(
            command.RoomId, command.StartDateTime, command.EndDateTime, cancellationToken: cancellationToken);
        if (hasConflict) return Fail(EShowError.RoomAlreadyBooked);

        // 4. Create and persist the show
        var show = new Show(command);
        await showRepository.AddAsync(show, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        // 5. Publish event
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
        if (show.Status != EShowStatus.Scheduled) return Fail(EShowError.ShowNotModifiable);

        if (!new TimeSlot(command.StartDateTime, command.EndDateTime).IsValid())
            return Fail(EShowError.InvalidTimeSlot);

        // The new slot must not fall inside a room block
        var slot = await roomsFacade.FetchRoomSlotAvailability(
            show.RoomId, command.StartDateTime, command.EndDateTime, cancellationToken);
        if (slot.IsBlocked) return Fail(EShowError.RoomBlocked);

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

        // Idempotent: cancelling twice is a no-op and must not publish a second event
        if (show.Status == EShowStatus.Cancelled) return Ok(show);
        if (show.Status == EShowStatus.Finished) return Fail(EShowError.ShowNotModifiable);

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