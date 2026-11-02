namespace KinemoPlatform.API.Maintenance.Application.Errors;

/// <summary>
///     Expected failures of the Maintenance Orders command service. The REST layer maps each value to an HTTP response.
/// </summary>
public enum EMaintenanceOrderError
{
    /// <summary>
    ///     The equipment the order refers to does not exist. The request body is invalid, so this is a
    ///     400 Bad Request (the route itself is valid).
    /// </summary>
    EquipmentNotFound,

    /// <summary>
    ///     The maintenance order does not exist (HTTP 404).
    /// </summary>
    OrderNotFound
}
