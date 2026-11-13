using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Model.Entities;
using KinemoPlatform.API.Seats.Domain.Model.Queries;
using KinemoPlatform.API.Seats.Domain.Repositories;
using KinemoPlatform.API.Seats.Application.Services;

namespace KinemoPlatform.API.Seats.Application.Internal.QueryServices;

/// <summary>
///     Implementation of the seat allocation query service.
/// </summary>
public class SeatAllocationQueryService(ISeatAllocationRepository seatAllocationRepository) : ISeatAllocationQueryService
{
    public async Task<SeatAllocation?> Handle(GetSeatAllocationByShowIdQuery query, CancellationToken cancellationToken = default)
    {
        return await seatAllocationRepository.FindByShowIdWithDetailsAsync(query.ShowId, cancellationToken);
    }

    public async Task<ShowSeat?> Handle(GetShowSeatQuery query, CancellationToken cancellationToken = default)
    {
        var allocation = await seatAllocationRepository.FindByShowIdWithDetailsAsync(query.ShowId, cancellationToken);
        return allocation?.ShowSeats.FirstOrDefault(s => s.SeatNumber == query.SeatNumber);
    }

}