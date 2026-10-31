using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Rooms.Domain.Model.Commands;

namespace KinemoPlatform.API.Rooms.Application.Services;

/// <summary>
///     Represents the room command service in the Kinemo Platform.
/// </summary>
public interface IRoomCommandService
{
    /// <summary>
    ///     Handles the creation of a new room.
    /// </summary>
    Task<Room?> Handle(CreateRoomCommand command);

    /// <summary>
    ///     Handles the preparation of a room (readiness inspection).
    /// </summary>
    Task<Room?> Handle(PrepareRoomCommand command);

    /// <summary>
    ///     Handles blocking a room for maintenance.
    /// </summary>
    Task<Room?> Handle(BlockRoomCommand command);

    /// <summary>
    ///     Handles releasing a room from maintenance.
    /// </summary>
    Task<Room?> Handle(ReleaseRoomCommand command);
}