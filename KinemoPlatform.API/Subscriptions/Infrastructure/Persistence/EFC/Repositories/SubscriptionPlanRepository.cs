using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionPlanRepository(AppDbContext context) 
    : BaseRepository<SubscriptionPlan>(context), ISubscriptionPlanRepository
{
    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await Context.Set<SubscriptionPlan>().AnyAsync(p => p.Name == name);
    }
}