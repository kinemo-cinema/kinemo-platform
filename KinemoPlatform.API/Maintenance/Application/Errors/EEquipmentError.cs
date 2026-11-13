namespace KinemoPlatform.API.Maintenance.Application.Errors;

/// <summary>
///     Expected failures of the Equipment command service. The REST layer maps each value to an HTTP response.
/// </summary>
public enum EEquipmentError
{
    /// <summary>
    ///     The equipment does not exist (HTTP 404).
    /// </summary>
    EquipmentNotFound,

    /// <summary>
    ///     The room already has equipment with the same name (HTTP 409).
    /// </summary>
    DuplicateEquipment
}
