using KinemoPlatform.API.Ticketing.Domain.Model.Entities;
using KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Transform;

public static class ShowOccupancyResourceFromEntityAssembler
{
    public static ShowOccupancyResource ToResourceFromEntity(ShowOccupancy entity)
    {
        return new ShowOccupancyResource(entity.Id, entity.ShowId, entity.TotalSeats, entity.OccupiedSeats, entity.SynchronizedAt);
    }
}