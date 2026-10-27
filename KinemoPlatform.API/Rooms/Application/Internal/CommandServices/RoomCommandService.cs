using Cortex.Mediator;
using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Rooms.Domain.Model.Commands;
using KinemoPlatform.API.Rooms.Domain.Model.Events;
using KinemoPlatform.API.Rooms.Domain.Repositories;
using KinemoPlatform.API.Rooms.Domain.Services;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Rooms.Application.Internal.CommandServices;

/// <summary>
///     Implementation of the room command service.
/// </summary>
public class RoomCommandService(
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IRoomCommandService
{
    public async Task<Room?> Handle(CreateRoomCommand command)
    {
        if (await roomRepository.ExistsByNameAsync(command.Name))
            throw new InvalidOperationException($"A room with the name '{command.Name}' already exists.");

        var room = new Room(command);
        await roomRepository.AddAsync(room);
        await unitOfWork.CompleteAsync();

        return room;
    }

    public async Task<Room?> Handle(PrepareRoomCommand command)
    {
        var room = await roomRepository.FindByIdWithDetailsAsync(command.RoomId);
        if (room is null) throw new InvalidOperationException($"Room with id {command.RoomId} was not found.");

        room.Prepare(command.Observations);
        roomRepository.Update(room);
        await unitOfWork.CompleteAsync();

        // Get the ID of the newly created inspection to pass to the event
        var latestInspection = room.Inspections.OrderByDescending(i => i.InspectedAt).First();
        
        await domainEventPublisher.PublishAsync(new RoomPreparedEvent(room.Id, latestInspection.Id));

        return room;
    }

    public async Task<Room?> Handle(BlockRoomCommand command)
    {
        var room = await roomRepository.FindByIdWithDetailsAsync(command.RoomId);
        if (room is null) throw new InvalidOperationException($"Room with id {command.RoomId} was not found.");

        room.Block(command.Reason, command.StartDateTime, command.EndDateTime);
        roomRepository.Update(room);
        await unitOfWork.CompleteAsync();

        var latestBlock = room.Blocks.OrderByDescending(b => b.StartDateTime).First();

        await domainEventPublisher.PublishAsync(new RoomBlockedEvent(room.Id, latestBlock.Id, latestBlock.Reason));

        return room;
    }

    public async Task<Room?> Handle(ReleaseRoomCommand command)
    {
        var room = await roomRepository.FindByIdWithDetailsAsync(command.RoomId);
        if (room is null) throw new InvalidOperationException($"Room with id {command.RoomId} was not found.");

        room.Release();
        roomRepository.Update(room);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(new RoomReleasedEvent(room.Id));

        return room;
    }
}