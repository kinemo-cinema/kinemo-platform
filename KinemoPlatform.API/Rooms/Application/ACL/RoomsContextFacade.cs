using KinemoPlatform.API.Rooms.Application.Services;
using KinemoPlatform.API.Rooms.Domain.Model.Queries;
using KinemoPlatform.API.Rooms.Interfaces.ACL;

namespace KinemoPlatform.API.Rooms.Application.ACL;

public class RoomsContextFacade(IRoomQueryService roomQueryService) : IRoomsContextFacade
{
    public async Task<IReadOnlyList<int>> FetchRoomIds(CancellationToken cancellationToken = default)
    {
        var rooms = await roomQueryService.Handle(new GetAllRoomsQuery(), cancellationToken);
        return rooms.Select(room => room.Id).ToList();
    }
    
    public async Task<int> FetchRoomCapacity(int roomId, CancellationToken cancellationToken = default)
    {
        var room = await roomQueryService.Handle(new GetRoomByIdQuery(roomId), cancellationToken);
        return room?.Capacity ?? 0;
    }
    
    public async Task<RoomSlotAvailability> FetchRoomSlotAvailability(
        int roomId, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default)
    {
        // GetRoomByIdQuery loads the blocks, so no extra query is needed.
        var room = await roomQueryService.Handle(new GetRoomByIdQuery(roomId), cancellationToken);
        if (room is null) return new RoomSlotAvailability(false, false);

        var isBlocked = room.Blocks.Any(block =>
            block.Active && block.StartDateTime < to && block.EndDateTime > from);
        return new RoomSlotAvailability(true, isBlocked);
    }
}