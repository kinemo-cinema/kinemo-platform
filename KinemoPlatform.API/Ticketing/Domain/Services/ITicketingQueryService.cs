using KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;
using KinemoPlatform.API.Ticketing.Domain.Model.Entities;
using KinemoPlatform.API.Ticketing.Domain.Model.Queries;

namespace KinemoPlatform.API.Ticketing.Domain.Services;

public interface ITicketingQueryService
{
    Task<IEnumerable<TicketingIntegration>> Handle(GetAllIntegrationsQuery query);

    Task<TicketingIntegration?> Handle(GetIntegrationByIdQuery query);
    
    Task<ShowOccupancy?> Handle(GetOccupancyByShowIdQuery query);
}