using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;
using KinemoPlatform.API.Ticketing.Domain.Model.Events;

namespace KinemoPlatform.API.Ticketing.Application.Internal.EventHandlers;

public class OccupancySynchronizedEventHandler : IEventHandler<OccupancySynchronizedEvent>
{
    public Task Handle(OccupancySynchronizedEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            "SYNC SUCCESS: Integration {0} synchronized Show {1}. Occupied Seats: {2}",
            domainEvent.IntegrationId, domainEvent.ShowId, domainEvent.OccupiedSeats);
        return Task.CompletedTask;
    }
}