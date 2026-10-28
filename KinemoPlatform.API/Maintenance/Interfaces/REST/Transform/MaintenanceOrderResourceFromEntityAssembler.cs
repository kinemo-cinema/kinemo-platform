using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
public static class MaintenanceOrderResourceFromEntityAssembler
{
    public static MaintenanceOrderResource ToResourceFromEntity(MaintenanceOrder e) => 
        new(e.Id, e.EquipmentId, e.IncidentId, e.MaintenanceType, e.Status, e.ScheduledDate, e.CompletedAt, e.CompletionNotes);
}