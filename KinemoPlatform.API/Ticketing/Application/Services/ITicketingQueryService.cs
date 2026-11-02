using KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;
using KinemoPlatform.API.Ticketing.Domain.Model.Entities;
using KinemoPlatform.API.Ticketing.Domain.Model.Queries;

namespace KinemoPlatform.API.Ticketing.Application.Services;

public interface ITicketingQueryService
{
    Task<TicketingIntegration?> Handle(GetIntegrationByIdQuery query, CancellationToken cancellationToken = default);
    
    Task<ShowOccupancy?> Handle(GetOccupancyByShowIdQuery query, CancellationToken cancellationToken = default);
}