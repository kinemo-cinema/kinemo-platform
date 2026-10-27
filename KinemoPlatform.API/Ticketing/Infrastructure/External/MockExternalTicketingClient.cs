using KinemoPlatform.API.Ticketing.Domain.ExternalServices;

namespace KinemoPlatform.API.Ticketing.Infrastructure.External;

/// <summary>
///     Mock implementation of the external ticketing client.
/// </summary>
/// <remarks>
///     In a production environment, this class would make HTTP requests to 
///     a real external provider (e.g., Teleticket or Joinn). 
///     For demonstration purposes, this mock simulates network latency
///     and returns deterministic data based on the show ID.
/// </remarks>
public class MockExternalTicketingClient : IExternalTicketingClient
{
    public async Task<ExternalOccupancyData?> GetOccupancyAsync(string providerName, int showId)
    {
        // Simulate network latency (e.g., 500ms)
        await Task.Delay(500);

        // Simulate a failure for invalid show IDs (e.g., 999)
        if (showId == 999)
            throw new HttpRequestException("External provider timed out.");

        // Simulate deterministic data based on the show ID
        var totalSeats = 100;
        var occupiedSeats = showId * 10; 

        // Ensure we don't exceed total seats
        if (occupiedSeats > totalSeats) occupiedSeats = totalSeats;

        return new ExternalOccupancyData(totalSeats, occupiedSeats);
    }
}