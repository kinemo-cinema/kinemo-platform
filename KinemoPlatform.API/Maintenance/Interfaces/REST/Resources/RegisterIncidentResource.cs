using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;

public record RegisterIncidentResource(string Description, EIncidentSeverity Severity);