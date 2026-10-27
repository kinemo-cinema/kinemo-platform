using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
public record MaintenanceOrderResource(
    int Id, 
    int EquipmentId, 
    int? IncidentId, 
    EMaintenanceType MaintenanceType, 
    EMaintenanceStatus Status, 
    DateTimeOffset ScheduledDate, 
    DateTimeOffset? CompletedAt, 
    string? CompletionNotes);