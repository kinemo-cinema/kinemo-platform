namespace KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

/// <summary>
///     Represents the progress of a maintenance order.
/// </summary>
public enum EMaintenanceStatus
{
    Created,
    Scheduled,
    InProgress,
    Completed,
    Cancelled
}