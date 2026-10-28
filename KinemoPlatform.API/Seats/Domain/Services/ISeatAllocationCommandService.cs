using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Model.Commands;

namespace KinemoPlatform.API.Seats.Domain.Services;

/// <summary>
///     Represents the seat allocation command service in the Kinemo Platform.
/// </summary>
public interface ISeatAllocationCommandService
{
    /// <summary>
    ///     Handles the initialization of a seat map for a new show.
    /// </summary>
    Task<SeatAllocation?> Handle(InitializeSeatsCommand command);

    /// <summary>
    ///     Handles assigning a specific status to a seat.
    /// </summary>
    Task<SeatAllocation?> Handle(AssignSeatCommand command);

    /// <summary>
    ///     Handles marking a seat as out of service.
    /// </summary>
    Task<SeatAllocation?> Handle(MarkSeatOutOfServiceCommand command);
}