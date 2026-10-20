namespace KinemoPlatform.API.Ticketing.Domain.Model.ValueObjects;

/// <summary>
///     Represents the connection status of a ticketing integration.
/// </summary>
public enum EIntegrationStatus
{
    Connected,
    Disconnected,
    Error
}