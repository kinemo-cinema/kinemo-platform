using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Rooms.Domain.Model.Queries;

namespace KinemoPlatform.API.Rooms.Application.Services;

/// <summary>
///     Represents the room query service in the Kinemo Platform.
/// </summary>
public interface IRoomQueryService
{
    /// <summary>
    ///     Handles retrieving a room by its ID.
    /// </summary>
    Task<Room?> Handle(GetRoomByIdQuery query, CancellationToken cancellationToken = default);


    /// <summary>
    ///     Handles retrieving all rooms.
    /// </summary>
    Task<IEnumerable<Room>> Handle(GetAllRoomsQuery query, CancellationToken cancellationToken = default);
}