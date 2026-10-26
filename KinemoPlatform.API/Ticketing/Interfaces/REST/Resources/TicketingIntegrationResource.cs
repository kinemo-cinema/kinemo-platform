using KinemoPlatform.API.Ticketing.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;

public record TicketingIntegrationResource(
    int Id,
    int CinemaId,
    string ProviderName,
    EIntegrationStatus Status,
    DateTimeOffset? LastConnectionAt,
    IEnumerable<SyncLogResource> SyncLogs,
    IEnumerable<ShowOccupancyResource> Occupancies);