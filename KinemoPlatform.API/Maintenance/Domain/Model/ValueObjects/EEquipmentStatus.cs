namespace KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

/// <summary>
///     Represents the operational state of the equipment.
/// </summary>
public enum EEquipmentStatus
{
    Operational,
    Testing,
    Maintenance,
    Faulty,
    OutOfService
}