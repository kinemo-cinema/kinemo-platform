using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Commands;

public record RegisterIncidentCommand(int EquipmentId, string Description, EIncidentSeverity Severity);