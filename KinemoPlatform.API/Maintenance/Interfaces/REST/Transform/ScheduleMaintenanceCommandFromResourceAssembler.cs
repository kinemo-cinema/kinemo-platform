using KinemoPlatform.API.Maintenance.Domain.Model.Commands;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
public static class ScheduleMaintenanceCommandFromResourceAssembler
{
    public static ScheduleMaintenanceCommand ToCommandFromResource(ScheduleMaintenanceResource r) => 
        new(r.EquipmentId, r.IncidentId, r.Type, r.ScheduledDate);
}