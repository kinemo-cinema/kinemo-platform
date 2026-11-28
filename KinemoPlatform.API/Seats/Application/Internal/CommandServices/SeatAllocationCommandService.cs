using KinemoPlatform.API.Rooms.Interfaces.ACL;
using KinemoPlatform.API.Scheduling.Interfaces.ACL;
using KinemoPlatform.API.Seats.Application.Errors;
using KinemoPlatform.API.Seats.Application.Services;
using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Model.Commands;
using KinemoPlatform.API.Seats.Domain.Model.ValueObjects;
using KinemoPlatform.API.Seats.Domain.Repositories;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Seats.Application.Internal.CommandServices;

public class SeatAllocationCommandService(
    ISeatAllocationRepository seatAllocationRepository,
    ISchedulingContextFacade schedulingFacade,
    IRoomsContextFacade roomsFacade,
    IUnitOfWork unitOfWork) : ISeatAllocationCommandService
{
    public async Task<Result<SeatAllocation, ESeatAllocationError>> Handle(InitializeSeatsCommand command,
        CancellationToken cancellationToken = default)
    {
        var show = await schedulingFacade.FetchShowInfo(command.ShowId, cancellationToken);
        if (show is null) return Fail(ESeatAllocationError.ShowNotFound);
        if (!show.IsScheduled) return Fail(ESeatAllocationError.ShowNotScheduled);

        if (await seatAllocationRepository.ExistsByShowIdAsync(command.ShowId, cancellationToken))
            return Fail(ESeatAllocationError.AllocationAlreadyExists);

        var capacity = await roomsFacade.FetchRoomCapacity(show.RoomId, cancellationToken);
        if (capacity <= 0) return Fail(ESeatAllocationError.RoomCapacityUnavailable);

        var allocation = new SeatAllocation(command.ShowId, capacity);
        await seatAllocationRepository.AddAsync(allocation, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Ok(allocation);
    }

    public async Task<Result<SeatAllocation, ESeatAllocationError>> Handle(AssignSeatCommand command,
        CancellationToken cancellationToken = default)
    {
        var allocation = await seatAllocationRepository.FindByShowIdWithDetailsAsync(command.ShowId, cancellationToken);
        if (allocation is null) return Fail(ESeatAllocationError.AllocationNotFound);
        if (allocation.IsClosed) return Fail(ESeatAllocationError.AllocationClosed);
        if (!allocation.HasSeat(command.SeatNumber)) return Fail(ESeatAllocationError.SeatNotFound);
        if (!SeatAllocation.IsManuallyAssignable(command.NewStatus))
            return Fail(ESeatAllocationError.UnsupportedSeatStatus);

        if (command.NewStatus == ESeatStatus.OutOfService)
            allocation.MarkSeatOutOfService(command.SeatNumber,
                string.IsNullOrWhiteSpace(command.Reason) ? "Marked out of service" : command.Reason);
        else
            allocation.AssignSeat(command.SeatNumber, command.NewStatus, command.Reason);
        
        seatAllocationRepository.Update(allocation);
        await unitOfWork.CompleteAsync(cancellationToken);   // dispatches SeatStatusChangedEvent
        return Ok(allocation);
    }

    public async Task<Result<SeatAllocation, ESeatAllocationError>> Handle(MarkSeatOutOfServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        var allocation = await seatAllocationRepository.FindByShowIdWithDetailsAsync(command.ShowId, cancellationToken);
        if (allocation is null) return Fail(ESeatAllocationError.AllocationNotFound);
        if (allocation.IsClosed) return Fail(ESeatAllocationError.AllocationClosed);
        if (!allocation.HasSeat(command.SeatNumber)) return Fail(ESeatAllocationError.SeatNotFound);

        allocation.MarkSeatOutOfService(command.SeatNumber, command.Reason);
        seatAllocationRepository.Update(allocation);
        await unitOfWork.CompleteAsync(cancellationToken);   // dispatches SeatMarkedOutOfServiceEvent
        return Ok(allocation);
    }

    public async Task<Result<SeatAllocation, ESeatAllocationError>> Handle(CloseAllocationCommand command,
        CancellationToken cancellationToken = default)
    {
        var allocation = await seatAllocationRepository.FindByShowIdWithDetailsAsync(command.ShowId, cancellationToken);
        if (allocation is null) return Fail(ESeatAllocationError.AllocationNotFound);

        // The entity is already tracked, so EF detects the status change without Update().
        // Update() would mark every seat row as modified for no reason.
        allocation.Close();
        await unitOfWork.CompleteAsync(cancellationToken);
        return Ok(allocation);
    }

    public async Task<Result<SeatAllocation, ESeatAllocationError>> Handle(MarkAllocationSynchronizedCommand command,
        CancellationToken cancellationToken = default)
    {
        var allocation = await seatAllocationRepository.FindByShowIdWithDetailsAsync(command.ShowId, cancellationToken);
        if (allocation is null) return Fail(ESeatAllocationError.AllocationNotFound);

        allocation.MarkSynchronized();
        await unitOfWork.CompleteAsync(cancellationToken);
        return Ok(allocation);
    }

    private static Result<SeatAllocation, ESeatAllocationError> Ok(SeatAllocation allocation) =>
        new Result<SeatAllocation, ESeatAllocationError>.Success(allocation);

    private static Result<SeatAllocation, ESeatAllocationError> Fail(ESeatAllocationError error) =>
        new Result<SeatAllocation, ESeatAllocationError>.Failure(error);
}