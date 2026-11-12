using KinemoPlatform.API.Ticketing.Domain.ExternalServices;

namespace KinemoPlatform.API.Ticketing.Infrastructure.External;

public class UnconfiguredExternalTicketingClient : IExternalTicketingClient
{
    public Task<ExternalOccupancyData?> GetOccupancyAsync(string providerName, int showId) =>
        Task.FromException<ExternalOccupancyData?>(new HttpRequestException("No real external ticketing provider is configured."));
}
