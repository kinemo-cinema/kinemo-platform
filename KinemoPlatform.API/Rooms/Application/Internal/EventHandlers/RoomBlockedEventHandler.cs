using KinemoPlatform.API.Rooms.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Rooms.Application.Internal.EventHandlers;

public class RoomBlockedEventHandler : IEventHandler<RoomBlockedEvent>
{
    public Task Handle(RoomBlockedEvent domainEvent, CancellationToken cancellationToken)
    {
        return On(domainEvent);
    }

    private static Task On(RoomBlockedEvent domainEvent)
    {
        Console.WriteLine("Room {0} was blocked. Reason: {1} (Block ID: {2})", domainEvent.RoomId, domainEvent.Reason, domainEvent.BlockId);
        return Task.CompletedTask;
    }
}