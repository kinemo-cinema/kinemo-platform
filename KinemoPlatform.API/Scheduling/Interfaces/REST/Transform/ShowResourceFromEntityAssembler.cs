using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Scheduling.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Scheduling.Interfaces.REST.Transform;

public static class ShowResourceFromEntityAssembler
{
    public static ShowResource ToResourceFromEntity(Show entity)
    {
        return new ShowResource(
            entity.Id,
            entity.MovieId,
            entity.RoomId,
            entity.TimeSlot.StartDateTime,
            entity.TimeSlot.EndDateTime,
            entity.Status);
    }
}