using KinemoPlatform.API.Rooms.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Rooms.Application.Internal.EventHandlers;

public class RoomReleasedEventHandler : IEventHandler<RoomReleasedEvent>
{
    public Task Handle(RoomReleasedEvent domainEvent, CancellationToken cancellationToken)
    {
        return On(domainEvent);
    }

    private static Task On(RoomReleasedEvent domainEvent)
    {
        Console.WriteLine("Room {0} was released and is now Available.", domainEvent.RoomId);
        return Task.CompletedTask;
    }
}