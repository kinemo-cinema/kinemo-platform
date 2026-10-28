using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
public record IncidentResource(int Id, string Description, EIncidentSeverity Severity, EIncidentStatus Status, DateTimeOffset ReportedAt, DateTimeOffset? ResolvedAt);