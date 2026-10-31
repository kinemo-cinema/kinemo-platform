using KinemoPlatform.API.Ticketing.Application.Services;
using KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;
using KinemoPlatform.API.Ticketing.Domain.Model.Entities;
using KinemoPlatform.API.Ticketing.Domain.Model.Queries;
using KinemoPlatform.API.Ticketing.Domain.Repositories;

namespace KinemoPlatform.API.Ticketing.Application.Internal.QueryServices;

public class TicketingQueryService(ITicketingRepository ticketingRepository) : ITicketingQueryService
{
    public async Task<TicketingIntegration?> Handle(GetIntegrationByIdQuery query)
    {
        return await ticketingRepository.FindByIdWithDetailsAsync(query.IntegrationId);
    }

    public async Task<ShowOccupancy?> Handle(GetOccupancyByShowIdQuery query)
    {
        return await ticketingRepository.FindOccupancyByShowIdAsync(query.ShowId);
    }
}