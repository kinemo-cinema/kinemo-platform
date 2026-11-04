using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;
using KinemoPlatform.API.Ticketing.Domain.Model.Entities;
using KinemoPlatform.API.Ticketing.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Ticketing.Infrastructure.Persistence.EFC.Repositories;

public class TicketingRepository(AppDbContext context) 
    : BaseRepository<TicketingIntegration>(context), ITicketingRepository
{
    public async Task<TicketingIntegration?> FindByIdWithDetailsAsync(int integrationId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TicketingIntegration>()
            .Include(t => t.SyncLogs)
            .Include(t => t.Occupancies)
            .FirstOrDefaultAsync(t => t.Id == integrationId, cancellationToken);
    }

    public async Task<ShowOccupancy?> FindOccupancyByShowIdAsync(int showId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<ShowOccupancy>()
            .FirstOrDefaultAsync(o => o.ShowId == showId, cancellationToken);
    }
}