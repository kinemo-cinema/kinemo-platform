namespace KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

/// <summary>
///     Distinguishes between scheduled upkeep and fixing a broken part.
/// </summary>
public enum EMaintenanceType
{
    Preventive,
    Corrective
}