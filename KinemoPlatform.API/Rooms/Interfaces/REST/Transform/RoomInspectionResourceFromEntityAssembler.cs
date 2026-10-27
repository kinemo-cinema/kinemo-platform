using KinemoPlatform.API.Rooms.Domain.Model.Entities;
using KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Transform;

public static class RoomInspectionResourceFromEntityAssembler
{
    public static RoomInspectionResource ToResourceFromEntity(RoomInspection entity)
    {
        return new RoomInspectionResource(entity.Id, entity.InspectedAt, entity.Observations, entity.Result);
    }
}