using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Rooms.Domain.Model.Queries;
using KinemoPlatform.API.Rooms.Domain.Repositories;
using KinemoPlatform.API.Rooms.Domain.Services;

namespace KinemoPlatform.API.Rooms.Application.Internal.QueryServices;

/// <summary>
///     Implementation of the room query service.
/// </summary>
public class RoomQueryService(IRoomRepository roomRepository) : IRoomQueryService
{
    public async Task<Room?> Handle(GetRoomByIdQuery query)
    {
        // Use the details method to eagerly load inspections and blocks
        return await roomRepository.FindByIdWithDetailsAsync(query.RoomId);
    }

    public async Task<IEnumerable<Room>> Handle(GetAllRoomsQuery query)
    {
        return await roomRepository.ListAsync();
    }
}