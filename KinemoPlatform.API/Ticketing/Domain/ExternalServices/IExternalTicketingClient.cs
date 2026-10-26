namespace KinemoPlatform.API.Ticketing.Domain.ExternalServices;

/// <summary>
///     Data Transfer Object representing the occupancy data returned by the external system.
/// </summary>
public record ExternalOccupancyData(int TotalSeats, int OccupiedSeats);

/// <summary>
///     Port for communicating with the external ticketing provider (e.g., Teleticket, Joinn).
/// </summary>
public interface IExternalTicketingClient
{
    /// <summary>
    ///     Fetches the current seat occupancy for a specific show from the external system.
    /// </summary>
    /// <param name="providerName">The name of the provider (e.g., "Teleticket").</param>
    /// <param name="showId">The ID of the show in our system.</param>
    /// <returns>The occupancy data, or null if the show is not found externally.</returns>
    Task<ExternalOccupancyData?> GetOccupancyAsync(string providerName, int showId);
}