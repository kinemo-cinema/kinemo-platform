using Cortex.Mediator;
using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Model.Commands;
using KinemoPlatform.API.Seats.Domain.Model.Events;
using KinemoPlatform.API.Seats.Domain.Repositories;
using KinemoPlatform.API.Seats.Domain.Services;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Seats.Application.Internal.CommandServices;

/// <summary>
///     Implementation of the seat allocation command service.
/// </summary>
public class SeatAllocationCommandService(
    ISeatAllocationRepository seatAllocationRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : ISeatAllocationCommandService
{
    public async Task<SeatAllocation?> Handle(InitializeSeatsCommand command)
    {
        if (await seatAllocationRepository.ExistsByShowIdAsync(command.ShowId))
            throw new InvalidOperationException($"A seat allocation for Show {command.ShowId} already exists.");

        var allocation = new SeatAllocation(command);
        await seatAllocationRepository.AddAsync(allocation);
        await unitOfWork.CompleteAsync();

        return allocation;
    }

    public async Task<SeatAllocation?> Handle(AssignSeatCommand command)
    {
        var allocation = await seatAllocationRepository.FindByShowIdWithDetailsAsync(command.ShowId);
        if (allocation is null) throw new InvalidOperationException($"Seat allocation for Show {command.ShowId} was not found.");

        allocation.AssignSeat(command.SeatNumber, command.NewStatus);
        seatAllocationRepository.Update(allocation);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new SeatStatusChangedEvent(allocation.ShowId, command.SeatNumber, command.NewStatus));

        return allocation;
    }

    public async Task<SeatAllocation?> Handle(MarkSeatOutOfServiceCommand command)
    {
        var allocation = await seatAllocationRepository.FindByShowIdWithDetailsAsync(command.ShowId);
        if (allocation is null) throw new InvalidOperationException($"Seat allocation for Show {command.ShowId} was not found.");

        allocation.MarkSeatOutOfService(command.SeatNumber, command.Reason);
        seatAllocationRepository.Update(allocation);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new SeatMarkedOutOfServiceEvent(allocation.ShowId, command.SeatNumber, command.Reason));

        return allocation;
    }
}