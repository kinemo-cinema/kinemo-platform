using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Transform;

public static class SeatAllocationResourceFromEntityAssembler
{
    public static SeatAllocationResource ToResourceFromEntity(SeatAllocation entity)
    {
        return new SeatAllocationResource(
            entity.Id,
            entity.ShowId,
            entity.ShowSeats.OrderBy(s => s.SeatNumber).Select(ShowSeatResourceFromEntityAssembler.ToResourceFromEntity));
    }
}