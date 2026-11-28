using KinemoPlatform.API.Scheduling.Domain.Model.Events;
using KinemoPlatform.API.Seats.Application.Errors;
using KinemoPlatform.API.Seats.Application.Services;
using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Model.Commands;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;
using KinemoPlatform.API.Shared.Application.Patterns;

namespace KinemoPlatform.API.Seats.Application.Internal.EventHandlers;

public class CloseAllocationOnShowCancelledEventHandler(
    ISeatAllocationCommandService commandService,
    ILogger<CloseAllocationOnShowCancelledEventHandler> logger) : IEventHandler<ShowCancelledEvent>
{
    public async Task Handle(ShowCancelledEvent domainEvent, CancellationToken cancellationToken)
    {
        // The show is already cancelled and saved. Never rethrow: that would fail a successful request.
        try
        {
            var result = await commandService.Handle(new CloseAllocationCommand(domainEvent.ShowId), cancellationToken);
            if (result is Result<SeatAllocation, ESeatAllocationError>.Failure failure &&
                failure.Error != ESeatAllocationError.AllocationNotFound)   // a show may have no allocation yet
            {
                logger.LogWarning("Allocation of show {ShowId} was not closed: {Error}", domainEvent.ShowId, failure.Error);
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Closing the allocation of show {ShowId} failed", domainEvent.ShowId);
        }
    }
}