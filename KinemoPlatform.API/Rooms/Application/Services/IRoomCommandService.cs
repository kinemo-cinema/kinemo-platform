using KinemoPlatform.API.Rooms.Application.Errors;
using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Rooms.Domain.Model.Commands;
using KinemoPlatform.API.Shared.Application.Patterns;

namespace KinemoPlatform.API.Rooms.Application.Services;

/// <summary>
///     Represents the room command service in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Every operation returns a success result with the affected <see cref="Room" />, or a failure with an
///     <see cref="ERoomError" />.
/// </remarks>
public interface IRoomCommandService
{
    /// <summary>
    ///     Handles the creation of a new room.
    /// </summary>
    Task<Result<Room, ERoomError>> Handle(CreateRoomCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles the preparation of a room (readiness inspection).
    /// </summary>
    Task<Result<Room, ERoomError>> Handle(PrepareRoomCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles blocking a room for maintenance.
    /// </summary>
    Task<Result<Room, ERoomError>> Handle(BlockRoomCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles releasing a room from maintenance.
    /// </summary>
    Task<Result<Room, ERoomError>> Handle(ReleaseRoomCommand command, CancellationToken cancellationToken = default);
}