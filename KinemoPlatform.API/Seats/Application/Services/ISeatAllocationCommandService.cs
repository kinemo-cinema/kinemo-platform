using KinemoPlatform.API.Seats.Application.Errors;
using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Model.Commands;
using KinemoPlatform.API.Shared.Application.Patterns;

namespace KinemoPlatform.API.Seats.Application.Services;

/// <summary>
///     Represents the seat allocation command service in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Every operation returns a success result with the affected <see cref="SeatAllocation" />, or a failure with an
///     <see cref="ESeatAllocationError" />.
/// </remarks>
public interface ISeatAllocationCommandService
{
    /// <summary>
    ///     Handles the generation of the seat map of a show.
    /// </summary>
    Task<Result<SeatAllocation, ESeatAllocationError>> Handle(InitializeSeatsCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles the manual change of a seat's status.
    /// </summary>
    Task<Result<SeatAllocation, ESeatAllocationError>> Handle(AssignSeatCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles marking a seat out of service (US12).
    /// </summary>
    Task<Result<SeatAllocation, ESeatAllocationError>> Handle(MarkSeatOutOfServiceCommand command,
        CancellationToken cancellationToken = default);
}
