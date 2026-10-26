using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Transform;

public static class RoomResourceFromEntityAssembler
{
    public static RoomResource ToResourceFromEntity(Room entity)
    {
        return new RoomResource(
            entity.Id,
            entity.Name,
            entity.Capacity,
            entity.Status,
            entity.Inspections.Select(RoomInspectionResourceFromEntityAssembler.ToResourceFromEntity),
            entity.Blocks.Select(RoomBlockResourceFromEntityAssembler.ToResourceFromEntity));
    }
}