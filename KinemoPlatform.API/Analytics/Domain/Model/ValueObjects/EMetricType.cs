namespace KinemoPlatform.API.Analytics.Domain.Model.ValueObjects;

/// <summary>
///     Represents the type of operational metric being tracked.
/// </summary>
public enum EMetricType
{
    RoomUtilization,
    OperatingHours,
    ShowCount,
    IncidentCount,
    MaintenanceCount
}