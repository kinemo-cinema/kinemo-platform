using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Model.Entities;
using KinemoPlatform.API.Seats.Domain.Model.Queries;
using KinemoPlatform.API.Seats.Domain.Repositories;
using KinemoPlatform.API.Seats.Domain.Services;

namespace KinemoPlatform.API.Seats.Application.Internal.QueryServices;

/// <summary>
///     Implementation of the seat allocation query service.
/// </summary>
public class SeatAllocationQueryService(ISeatAllocationRepository seatAllocationRepository) : ISeatAllocationQueryService
{
    public async Task<SeatAllocation?> Handle(GetSeatAllocationByShowIdQuery query)
    {
        return await seatAllocationRepository.FindByShowIdWithDetailsAsync(query.ShowId);
    }

    public async Task<ShowSeat?> Handle(GetShowSeatQuery query)
    {
        var allocation = await seatAllocationRepository.FindByShowIdWithDetailsAsync(query.ShowId);
        return allocation?.ShowSeats.FirstOrDefault(s => s.SeatNumber == query.SeatNumber);
    }
}