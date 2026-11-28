using KinemoPlatform.API.Seats.Application.Errors;
using KinemoPlatform.API.Seats.Application.Services;
using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Model.Commands;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Ticketing.Domain.Model.Events;

namespace KinemoPlatform.API.Seats.Application.Internal.EventHandlers;

public class MarkAllocationSynchronizedOnOccupancySynchronizedEventHandler(
    ISeatAllocationCommandService commandService,
    ILogger<MarkAllocationSynchronizedOnOccupancySynchronizedEventHandler> logger)
    : IEventHandler<OccupancySynchronizedEvent>
{
    public async Task Handle(OccupancySynchronizedEvent domainEvent, CancellationToken cancellationToken)
    {
        try
        {
            var result = await commandService.Handle(
                new MarkAllocationSynchronizedCommand(domainEvent.ShowId), cancellationToken);
            if (result is Result<SeatAllocation, ESeatAllocationError>.Failure failure &&
                failure.Error != ESeatAllocationError.AllocationNotFound)
            {
                logger.LogWarning("Allocation of show {ShowId} was not marked synchronized: {Error}", domainEvent.ShowId, failure.Error);
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Marking the allocation of show {ShowId} synchronized failed", domainEvent.ShowId);
        }
    }
}