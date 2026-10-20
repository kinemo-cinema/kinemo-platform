namespace KinemoPlatform.API.Ticketing.Domain.Model.Commands;

/// <summary>
///     Command to synchronize the occupancy of a specific show from the external ticketing system.
/// </summary>
/// <param name="IntegrationId">The ID of the integration to use.</param>
/// <param name="ShowId">The ID of the show to synchronize.</param>
public record SyncOccupancyCommand(int IntegrationId, int ShowId);