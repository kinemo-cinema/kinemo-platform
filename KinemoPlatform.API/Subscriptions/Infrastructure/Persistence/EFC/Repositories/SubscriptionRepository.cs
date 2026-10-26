using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionRepository(AppDbContext context) 
    : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    public async Task<Subscription?> FindByIdWithDetailsAsync(int subscriptionId)
    {
        return await Context.Set<Subscription>()
            .Include(s => s.Transactions)
            .Include(s => s.Upgrades)
            .FirstOrDefaultAsync(s => s.Id == subscriptionId);
    }

    public async Task<Subscription?> FindByCinemaIdAsync(int cinemaId)
    {
        return await Context.Set<Subscription>()
            .Include(s => s.Transactions)
            .Include(s => s.Upgrades)
            .Where(s => s.CinemaId == cinemaId)
            .OrderByDescending(s => s.EndDate) // Get the most recent/latest one
            .FirstOrDefaultAsync();
    }
}