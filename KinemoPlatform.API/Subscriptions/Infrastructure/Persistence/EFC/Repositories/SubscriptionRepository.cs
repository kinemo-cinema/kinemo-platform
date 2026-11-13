using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionRepository(AppDbContext context)
    : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    public async Task<IEnumerable<Subscription>> ListWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Subscription>().AsNoTracking()
            .Include(item => item.Transactions)
            .Include(item => item.Upgrades)
            .AsSplitQuery().ToListAsync(cancellationToken);
    }

    public async Task<Subscription?> FindByIdWithDetailsAsync(
        int subscriptionId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Subscription>()
            .Include(s => s.Transactions)
            .Include(s => s.Upgrades)
            .FirstOrDefaultAsync(
                s => s.Id == subscriptionId,
                cancellationToken);
    }

    public async Task<Subscription?> FindByCinemaIdAsync(
        int cinemaId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Subscription>()
            .Include(s => s.Transactions)
            .Include(s => s.Upgrades)
            .Where(s => s.CinemaId == cinemaId)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefaultAsync(cancellationToken);
    }
}