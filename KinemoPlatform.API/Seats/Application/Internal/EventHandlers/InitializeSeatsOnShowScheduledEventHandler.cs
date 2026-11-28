using KinemoPlatform.API.Scheduling.Domain.Model.Events;
using KinemoPlatform.API.Seats.Application.Errors;
using KinemoPlatform.API.Seats.Application.Services;
using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Model.Commands;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;
using KinemoPlatform.API.Shared.Application.Patterns;

namespace KinemoPlatform.API.Seats.Application.Internal.EventHandlers;

public class InitializeSeatsOnShowScheduledEventHandler(
    ISeatAllocationCommandService commandService,
    IConfiguration configuration,
    ILogger<InitializeSeatsOnShowScheduledEventHandler> logger) : IEventHandler<ShowScheduledEvent>
{
    public async Task Handle(ShowScheduledEvent domainEvent, CancellationToken cancellationToken)
    {
        if (!configuration.GetValue<bool>("Seats:AutoInitializeOnShowScheduled")) return;

        // The show is already saved when this runs. An exception here would turn a successful
        // scheduling request into a 500, so failures are logged and never rethrown.
        try
        {
            var result = await commandService.Handle(new InitializeSeatsCommand(domainEvent.ShowId), cancellationToken);
            if (result is Result<SeatAllocation, ESeatAllocationError>.Failure failure &&
                failure.Error != ESeatAllocationError.AllocationAlreadyExists)
            {
                logger.LogWarning("Seats were not initialized for show {ShowId}: {Error}", domainEvent.ShowId, failure.Error);
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Seat initialization failed for show {ShowId}", domainEvent.ShowId);
        }
    }
}