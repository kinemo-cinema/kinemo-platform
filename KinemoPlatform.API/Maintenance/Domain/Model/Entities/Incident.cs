using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Entities;

public class Incident
{
    public Incident()
    {
        Description = string.Empty;
    }

    public Incident(string description, EIncidentSeverity severity) : this()
    {
        Description = description;
        Severity = severity;
        Status = EIncidentStatus.Open;
        ReportedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; }
    public int EquipmentId { get; private set; }
    public string Description { get; private set; }
    public EIncidentSeverity Severity { get; private set; }
    public EIncidentStatus Status { get; private set; }
    public DateTimeOffset ReportedAt { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }

    public void Resolve()
    {
        Status = EIncidentStatus.Resolved;
        ResolvedAt = DateTimeOffset.UtcNow;
    }
}