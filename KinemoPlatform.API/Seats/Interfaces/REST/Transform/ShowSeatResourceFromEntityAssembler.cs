using KinemoPlatform.API.Seats.Domain.Model.Entities;
using KinemoPlatform.API.Seats.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Transform;

public static class ShowSeatResourceFromEntityAssembler
{
    public static ShowSeatResource ToResourceFromEntity(ShowSeat entity)
    {
        return new ShowSeatResource(entity.Id, entity.SeatNumber, entity.Sold, entity.Enabled, entity.Status);
    }
}