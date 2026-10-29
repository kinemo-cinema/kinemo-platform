using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Maintenance.Infrastructure.Persistence.EFC.Repositories;

public class EquipmentRepository(AppDbContext context) 
    : BaseRepository<Equipment>(context), IEquipmentRepository
{
    public async Task<bool> ExistsByNameAndRoomAsync(string name, int roomId)
    {
        return await Context.Set<Equipment>().AnyAsync(e => e.Name == name && e.RoomId == roomId);
    }

    public async Task<Equipment?> FindByIdWithDetailsAsync(int equipmentId)
    {
        return await Context.Set<Equipment>()
            .Include(e => e.Incidents)
            .Include(e => e.Tests)
            .ThenInclude(t => t.ChannelTests)
            .FirstOrDefaultAsync(e => e.Id == equipmentId);
    }

    // Override to include incidents and tests when listing all equipment
    public override async Task<IEnumerable<Equipment>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Equipment>()
            .Include(e => e.Incidents)
            .Include(e => e.Tests)
            .ThenInclude(t => t.ChannelTests)
            .ToListAsync(cancellationToken);
    }
}