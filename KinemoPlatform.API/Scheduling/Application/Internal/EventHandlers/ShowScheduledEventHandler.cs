using KinemoPlatform.API.Scheduling.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Scheduling.Application.Internal.EventHandlers;

public class ShowScheduledEventHandler : IEventHandler<ShowScheduledEvent>
{
    public Task Handle(ShowScheduledEvent domainEvent, CancellationToken cancellationToken)
    {
        return On(domainEvent);
    }

    private static Task On(ShowScheduledEvent domainEvent)
    {
        Console.WriteLine(
            "Scheduled Show: {0} for Movie {1} in Room {2} at {3}",
            domainEvent.ShowId,
            domainEvent.MovieId,
            domainEvent.RoomId,
            domainEvent.StartDateTime);
        return Task.CompletedTask;
    }
}