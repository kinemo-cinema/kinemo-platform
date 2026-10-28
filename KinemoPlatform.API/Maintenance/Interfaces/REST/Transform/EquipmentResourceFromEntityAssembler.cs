using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
public static class EquipmentResourceFromEntityAssembler
{
    public static EquipmentResource ToResourceFromEntity(Equipment e) => 
        new(e.Id, e.RoomId, e.Name, e.EquipmentType, e.Status, e.UsefulLifeHours, e.AccumulatedHours,
            e.Incidents.Select(IncidentResourceFromEntityAssembler.ToResourceFromEntity),
            e.Tests.Select(EquipmentTestResourceFromEntityAssembler.ToResourceFromEntity));
}