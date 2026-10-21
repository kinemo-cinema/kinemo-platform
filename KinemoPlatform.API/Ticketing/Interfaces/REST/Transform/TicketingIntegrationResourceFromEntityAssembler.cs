using KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;
using KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Transform;

public static class TicketingIntegrationResourceFromEntityAssembler
{
    public static TicketingIntegrationResource ToResourceFromEntity(TicketingIntegration entity)
    {
        return new TicketingIntegrationResource(
            entity.Id,
            entity.CinemaId,
            entity.ProviderName,
            entity.Status,
            entity.LastConnectionAt,
            entity.SyncLogs.Select(SyncLogResourceFromEntityAssembler.ToResourceFromEntity),
            entity.Occupancies.Select(ShowOccupancyResourceFromEntityAssembler.ToResourceFromEntity));
    }
}