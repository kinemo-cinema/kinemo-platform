using Cortex.Mediator;
using KinemoPlatform.API.Rooms.Application.Errors;
using KinemoPlatform.API.Rooms.Application.Services;
using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Rooms.Domain.Model.Commands;
using KinemoPlatform.API.Rooms.Domain.Model.Entities;
using KinemoPlatform.API.Rooms.Domain.Model.Events;
using KinemoPlatform.API.Rooms.Domain.Repositories;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Rooms.Application.Internal.CommandServices;

public class RoomCommandService(
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IRoomCommandService
{
    public async Task<Result<Room, ERoomError>> Handle(CreateRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await roomRepository.ExistsByNameAsync(command.Name, cancellationToken))
            return Fail<Room>(ERoomError.DuplicateName);

        var room = new Room(command);
        await roomRepository.AddAsync(room, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Ok(room);
    }

    // ---- Legacy handlers (deprecated routes): same behavior, now built on the shared cores below ----

    public async Task<Result<Room, ERoomError>> Handle(PrepareRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var (error, room, _) = await PrepareAsync(command.RoomId, command.Observations, cancellationToken);
        return error is ERoomError failure ? Fail<Room>(failure) : Ok(room!);
    }

    public async Task<Result<Room, ERoomError>> Handle(BlockRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var (error, room, _) = await BlockAsync(
            command.RoomId, command.Reason, command.StartDateTime, command.EndDateTime, cancellationToken);
        return error is ERoomError failure ? Fail<Room>(failure) : Ok(room!);
    }

    public async Task<Result<Room, ERoomError>> Handle(ReleaseRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.FindByIdWithDetailsAsync(command.RoomId, cancellationToken);
        if (room is null) return Fail<Room>(ERoomError.RoomNotFound);

        room.Release();
        roomRepository.Update(room);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(new RoomReleasedEvent(room.Id), cancellationToken);
        return Ok(room);
    }

    // ---- Handlers for the sub-resource routes ----

    public async Task<Result<RoomInspection, ERoomError>> Handle(CreateRoomInspectionCommand command,
        CancellationToken cancellationToken = default)
    {
        var (error, _, inspection) = await PrepareAsync(command.RoomId, command.Observations, cancellationToken);
        return error is ERoomError failure ? Fail<RoomInspection>(failure) : Ok(inspection!);
    }

    public async Task<Result<RoomBlock, ERoomError>> Handle(CreateRoomBlockCommand command,
        CancellationToken cancellationToken = default)
    {
        var (error, _, block) = await BlockAsync(
            command.RoomId, command.Reason, command.StartDateTime, command.EndDateTime, cancellationToken);
        return error is ERoomError failure ? Fail<RoomBlock>(failure) : Ok(block!);
    }

    public async Task<Result<RoomBlock, ERoomError>> Handle(LiftRoomBlockCommand command,
        CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.FindByIdWithDetailsAsync(command.RoomId, cancellationToken);
        if (room is null) return Fail<RoomBlock>(ERoomError.RoomNotFound);

        var block = room.Blocks.FirstOrDefault(b => b.Id == command.BlockId);
        if (block is null) return Fail<RoomBlock>(ERoomError.BlockNotFound);

        // Idempotent: lifting a block that is already lifted changes nothing and publishes nothing.
        if (!block.Active) return Ok(block);

        var roomIsAvailableAgain = room.LiftBlock(command.BlockId);

        // The room is already tracked, so EF detects the change without Update().
        await unitOfWork.CompleteAsync(cancellationToken);

        if (roomIsAvailableAgain)
            await domainEventPublisher.PublishAsync(new RoomReleasedEvent(room.Id), cancellationToken);

        return Ok(block);
    }

    // ---- Shared cores ----

    private async Task<(ERoomError? Error, Room? Room, RoomInspection? Inspection)> PrepareAsync(
        int roomId, string observations, CancellationToken cancellationToken)
    {
        var room = await roomRepository.FindByIdWithDetailsAsync(roomId, cancellationToken);
        if (room is null) return (ERoomError.RoomNotFound, null, null);

        // A blocked or out-of-service room must be released before it can be prepared again.
        if (!room.CanBePrepared()) return (ERoomError.RoomNotAvailableForPreparation, null, null);

        var inspection = room.Prepare(observations);
        roomRepository.Update(room);
        await unitOfWork.CompleteAsync(cancellationToken);

        // The inspection instance has its database id once the changes are saved.
        await domainEventPublisher.PublishAsync(new RoomPreparedEvent(room.Id, inspection.Id), cancellationToken);
        return (null, room, inspection);
    }

    private async Task<(ERoomError? Error, Room? Room, RoomBlock? Block)> BlockAsync(
        int roomId, string reason, DateTimeOffset startDateTime, DateTimeOffset endDateTime,
        CancellationToken cancellationToken)
    {
        // The block must end after it starts.
        if (endDateTime <= startDateTime) return (ERoomError.InvalidBlockPeriod, null, null);

        var room = await roomRepository.FindByIdWithDetailsAsync(roomId, cancellationToken);
        if (room is null) return (ERoomError.RoomNotFound, null, null);

        var block = room.Block(reason, startDateTime, endDateTime);
        roomRepository.Update(room);
        await unitOfWork.CompleteAsync(cancellationToken);

        // Use the block that was just created: ordering the blocks by start date could pick a different one.
        await domainEventPublisher.PublishAsync(new RoomBlockedEvent(room.Id, block.Id, block.Reason), cancellationToken);
        return (null, room, block);
    }

    private static Result<T, ERoomError> Ok<T>(T value) =>
        new Result<T, ERoomError>.Success(value);

    private static Result<T, ERoomError> Fail<T>(ERoomError error) =>
        new Result<T, ERoomError>.Failure(error);
}