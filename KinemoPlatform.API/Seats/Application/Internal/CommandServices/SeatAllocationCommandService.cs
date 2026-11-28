using Cortex.Mediator;
using KinemoPlatform.API.Rooms.Interfaces.ACL;
using KinemoPlatform.API.Scheduling.Interfaces.ACL;
using KinemoPlatform.API.Seats.Application.Errors;
using KinemoPlatform.API.Seats.Application.Services;
using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Model.Commands;
using KinemoPlatform.API.Seats.Domain.Model.Events;
using KinemoPlatform.API.Seats.Domain.Repositories;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Seats.Application.Internal.CommandServices;

/// <summary>
///     Implementation of the seat allocation command service.
/// </summary>
/// <remarks>
///     Expected failures are returned as <see cref="ESeatAllocationError" /> inside a
///     <see cref="Result{TValue,TError}" />. The service checks the preconditions of the aggregate (the seat exists, the
///     status is supported) before changing it, so the aggregate's own guards are never reached through the API.
///     Unexpected failures are not caught here; they surface as exceptions and are turned into a ProblemDetails 500
///     response globally.
/// </remarks>
public class SeatAllocationCommandService(
    ISeatAllocationRepository seatAllocationRepository,
    ISchedulingContextFacade schedulingFacade,
    IRoomsContextFacade roomsFacade,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : ISeatAllocationCommandService
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

        if (!allocation.HasSeat(command.SeatNumber)) return Fail(ESeatAllocationError.SeatNotFound);
        if (!SeatAllocation.IsManuallyAssignable(command.NewStatus))
            return Fail(ESeatAllocationError.UnsupportedSeatStatus);

        allocation.AssignSeat(command.SeatNumber, command.NewStatus);
        seatAllocationRepository.Update(allocation);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new SeatStatusChangedEvent(allocation.ShowId, command.SeatNumber, command.NewStatus),
            cancellationToken);

        return Ok(allocation);
    }

    public async Task<Result<SeatAllocation, ESeatAllocationError>> Handle(MarkSeatOutOfServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        var allocation = await seatAllocationRepository.FindByShowIdWithDetailsAsync(command.ShowId, cancellationToken);
        if (allocation is null) return Fail(ESeatAllocationError.AllocationNotFound);

        if (!allocation.HasSeat(command.SeatNumber)) return Fail(ESeatAllocationError.SeatNotFound);

        allocation.MarkSeatOutOfService(command.SeatNumber, command.Reason);
        seatAllocationRepository.Update(allocation);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new SeatMarkedOutOfServiceEvent(allocation.ShowId, command.SeatNumber, command.Reason),
            cancellationToken);

        return Ok(allocation);
    }

    private static Result<SeatAllocation, ESeatAllocationError> Ok(SeatAllocation allocation) =>
        new Result<SeatAllocation, ESeatAllocationError>.Success(allocation);

    private static Result<SeatAllocation, ESeatAllocationError> Fail(ESeatAllocationError error) =>
        new Result<SeatAllocation, ESeatAllocationError>.Failure(error);
}
