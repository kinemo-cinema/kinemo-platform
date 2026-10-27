using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Model.Entities;
using KinemoPlatform.API.Seats.Domain.Model.Queries;

namespace KinemoPlatform.API.Seats.Domain.Services;

/// <summary>
///     Represents the seat allocation query service in the Kinemo Platform.
/// </summary>
public interface ISeatAllocationQueryService
{
    /// <summary>
    ///     Handles retrieving the full seat map for a show.
    /// </summary>
    Task<SeatAllocation?> Handle(GetSeatAllocationByShowIdQuery query);

    /// <summary>
    ///     Handles retrieving a specific seat within a show.
    /// </summary>
    Task<ShowSeat?> Handle(GetShowSeatQuery query);
}