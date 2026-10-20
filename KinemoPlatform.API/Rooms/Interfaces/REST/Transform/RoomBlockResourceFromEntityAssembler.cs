using KinemoPlatform.API.Rooms.Domain.Model.Entities;
using KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Transform;

public static class RoomBlockResourceFromEntityAssembler
{
    public static RoomBlockResource ToResourceFromEntity(RoomBlock entity)
    {
        return new RoomBlockResource(entity.Id, entity.Reason, entity.StartDateTime, entity.EndDateTime, entity.Active);
    }
}