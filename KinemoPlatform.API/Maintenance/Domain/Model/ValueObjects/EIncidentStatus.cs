namespace KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

/// <summary>
///     Represents the lifecycle of a reported hardware incident.
/// </summary>
public enum EIncidentStatus
{
    Open,
    InProgress,
    Resolved,
    Closed
}