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
}