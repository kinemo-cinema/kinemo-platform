using KinemoPlatform.API.Rooms.Application.Services;
using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Rooms.Domain.Model.Queries;
using KinemoPlatform.API.Rooms.Domain.Repositories;

namespace KinemoPlatform.API.Rooms.Application.Internal.QueryServices;

/// <summary>
///     Implementation of the room query service.
/// </summary>
public class RoomQueryService(IRoomRepository roomRepository) : IRoomQueryService
{
    public async Task<Room?> Handle(GetRoomByIdQuery query, CancellationToken cancellationToken = default)
    {
        // Use the details method to eagerly load inspections and blocks
        return await roomRepository.FindByIdWithDetailsAsync(query.RoomId, cancellationToken);
    }

    public async Task<IEnumerable<Room>> Handle(GetAllRoomsQuery query, CancellationToken cancellationToken = default)
    {
        return await roomRepository.ListAsync(cancellationToken);
    }
}