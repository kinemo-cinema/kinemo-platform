using Cortex.Mediator;
using KinemoPlatform.API.Rooms.Application.Errors;
using KinemoPlatform.API.Rooms.Application.Services;
using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Rooms.Domain.Model.Commands;
using KinemoPlatform.API.Rooms.Domain.Model.Events;
using KinemoPlatform.API.Rooms.Domain.Repositories;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Rooms.Application.Internal.CommandServices;

/// <summary>
///     Implementation of the room command service.
/// </summary>
/// <remarks>
///     Expected failures (room not found, duplicate name) are returned as <see cref="ERoomError" /> inside a
///     <see cref="Result{TValue,TError}" />. Unexpected failures are not caught here; they surface as exceptions and
///     are turned into a ProblemDetails 500 response globally.
/// </remarks>
public class RoomCommandService(
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IRoomCommandService
{
    public async Task<Result<Room, ERoomError>> Handle(CreateRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await roomRepository.ExistsByNameAsync(command.Name, cancellationToken))
            return Fail(ERoomError.DuplicateName);

        var room = new Room(command);
        await roomRepository.AddAsync(room, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Ok(room);
    }

    public async Task<Result<Room, ERoomError>> Handle(PrepareRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.FindByIdWithDetailsAsync(command.RoomId, cancellationToken);
        if (room is null) return Fail(ERoomError.RoomNotFound);

        room.Prepare(command.Observations);
        roomRepository.Update(room);
        await unitOfWork.CompleteAsync(cancellationToken);

        // Get the ID of the newly created inspection to pass to the event
        var latestInspection = room.Inspections.OrderByDescending(i => i.InspectedAt).First();

        await domainEventPublisher.PublishAsync(new RoomPreparedEvent(room.Id, latestInspection.Id),
            cancellationToken);

        return Ok(room);
    }

    public async Task<Result<Room, ERoomError>> Handle(BlockRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.FindByIdWithDetailsAsync(command.RoomId, cancellationToken);
        if (room is null) return Fail(ERoomError.RoomNotFound);

        room.Block(command.Reason, command.StartDateTime, command.EndDateTime);
        roomRepository.Update(room);
        await unitOfWork.CompleteAsync(cancellationToken);

        var latestBlock = room.Blocks.OrderByDescending(b => b.StartDateTime).First();

        await domainEventPublisher.PublishAsync(new RoomBlockedEvent(room.Id, latestBlock.Id, latestBlock.Reason),
            cancellationToken);

        return Ok(room);
    }

    public async Task<Result<Room, ERoomError>> Handle(ReleaseRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.FindByIdWithDetailsAsync(command.RoomId, cancellationToken);
        if (room is null) return Fail(ERoomError.RoomNotFound);

        room.Release();
        roomRepository.Update(room);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(new RoomReleasedEvent(room.Id), cancellationToken);

        return Ok(room);
    }

    private static Result<Room, ERoomError> Ok(Room room) =>
        new Result<Room, ERoomError>.Success(room);

    private static Result<Room, ERoomError> Fail(ERoomError error) =>
        new Result<Room, ERoomError>.Failure(error);
}