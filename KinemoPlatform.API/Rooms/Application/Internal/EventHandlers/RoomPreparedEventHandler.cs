using KinemoPlatform.API.Rooms.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Rooms.Application.Internal.EventHandlers;

public class RoomPreparedEventHandler : IEventHandler<RoomPreparedEvent>
{
    public Task Handle(RoomPreparedEvent domainEvent, CancellationToken cancellationToken)
    {
        return On(domainEvent);
    }

    private static Task On(RoomPreparedEvent domainEvent)
    {
        Console.WriteLine("Room {0} was prepared. Inspection ID: {1}", domainEvent.RoomId, domainEvent.InspectionId);
        return Task.CompletedTask;
    }
}