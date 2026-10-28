using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Maintenance.Infrastructure.Persistence.EFC.Repositories;

public class MaintenanceOrderRepository(AppDbContext context) 
    : BaseRepository<MaintenanceOrder>(context), IMaintenanceOrderRepository
{
    public async Task<IEnumerable<MaintenanceOrder>> FindByEquipmentIdAsync(int equipmentId)
    {
        return await Context.Set<MaintenanceOrder>()
            .Where(m => m.EquipmentId == equipmentId)
            .OrderByDescending(m => m.ScheduledDate)
            .ToListAsync();
    }
}