using Cortex.Mediator;
using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Scheduling.Domain.Model.Commands;
using KinemoPlatform.API.Scheduling.Domain.Model.Events;
using KinemoPlatform.API.Scheduling.Domain.Repositories;
using KinemoPlatform.API.Scheduling.Domain.Services;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Scheduling.Application.Internal.CommandServices;

public class ShowCommandService(
    IShowRepository showRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IShowCommandService
{
    public async Task<Show?> Handle(ScheduleShowCommand command)
    {
        // 1. Check for room scheduling conflicts
        var hasConflict = await showRepository.ExistsOverlappingShowAsync(
            command.RoomId, command.StartDateTime, command.EndDateTime);

        if (hasConflict)
            throw new InvalidOperationException("The room is already booked for the selected time slot.");

        // 2. Create and persist the show
        var show = new Show(command);
        await showRepository.AddAsync(show);
        await unitOfWork.CompleteAsync();

        // 3. Publish event
        await domainEventPublisher.PublishAsync(
            new ShowScheduledEvent(show.Id, show.MovieId, show.RoomId, show.TimeSlot.StartDateTime));

        return show;
    }

    public async Task<Show?> Handle(EditShowCommand command)
    {
        var show = await showRepository.FindByIdAsync(command.ShowId);
        if (show is null) throw new InvalidOperationException("Show not found.");

        // Check for conflicts, excluding the show currently being edited
        var hasConflict = await showRepository.ExistsOverlappingShowAsync(
            show.RoomId, command.StartDateTime, command.EndDateTime, excludeShowId: show.Id);

        if (hasConflict)
            throw new InvalidOperationException("The new time slot overlaps with another show in this room.");

        show.UpdateTimeSlot(command.StartDateTime, command.EndDateTime);
        showRepository.Update(show);
        await unitOfWork.CompleteAsync();

        return show;
    }

    public async Task<Show?> Handle(CancelShowCommand command)
    {
        var show = await showRepository.FindByIdAsync(command.ShowId);
        if (show is null) throw new InvalidOperationException("Show not found.");

        show.Cancel();
        showRepository.Update(show);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(new ShowCancelledEvent(show.Id));

        return show;
    }
}