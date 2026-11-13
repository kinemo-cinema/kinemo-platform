using KinemoPlatform.API.Seats.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Seats.Application.Internal.EventHandlers;

public class SeatStatusChangedEventHandler : IEventHandler<SeatStatusChangedEvent>
{
    public Task Handle(SeatStatusChangedEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            "Seat Status Changed: Show {0}, Seat {1} is now {2}",
            domainEvent.ShowId, domainEvent.SeatNumber, domainEvent.NewStatus);
        return Task.CompletedTask;
    }
}