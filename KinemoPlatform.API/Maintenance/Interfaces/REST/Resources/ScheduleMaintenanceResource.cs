using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
public record ScheduleMaintenanceResource(int EquipmentId, int? IncidentId, EMaintenanceType Type, DateTimeOffset ScheduledDate);