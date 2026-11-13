using KinemoPlatform.API.Seats.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Seats.Application.Internal.EventHandlers;

public class SeatMarkedOutOfServiceEventHandler : IEventHandler<SeatMarkedOutOfServiceEvent>
{
    public Task Handle(SeatMarkedOutOfServiceEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            "ALERT: Seat {0} in Show {1} marked OUT OF SERVICE. Reason: {2}",
            domainEvent.SeatNumber, domainEvent.ShowId, domainEvent.Reason);
        return Task.CompletedTask;
    }
}