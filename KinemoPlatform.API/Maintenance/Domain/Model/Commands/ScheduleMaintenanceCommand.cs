using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Commands;

public record ScheduleMaintenanceCommand(int EquipmentId, int? IncidentId, EMaintenanceType Type, DateTimeOffset ScheduledDate);