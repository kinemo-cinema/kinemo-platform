using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Analytics.Infrastructure.Persistence.EFC.Repositories;

public class DashboardRepository(AppDbContext context)
    : BaseRepository<Dashboard>(context), IDashboardRepository
{
    public async Task<Dashboard?> FindByIdWithDetailsAsync(
        int dashboardId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Dashboard>()
            .Include(d => d.Metrics)
            .Include(d => d.RoomPerformances)
            .Include(d => d.IncidentSummaries)
            .FirstOrDefaultAsync(
                d => d.Id == dashboardId,
                cancellationToken);
    }

    public async Task<Dashboard?> FindLatestAsync(
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Dashboard>()
            .Include(d => d.Metrics)
            .Include(d => d.RoomPerformances)
            .Include(d => d.IncidentSummaries)
            .OrderByDescending(d => d.GeneratedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}