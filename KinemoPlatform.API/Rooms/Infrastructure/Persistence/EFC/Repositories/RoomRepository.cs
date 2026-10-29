using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Rooms.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Rooms.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
///     Implementation of the room repository.
/// </summary>
public class RoomRepository(AppDbContext context) 
    : BaseRepository<Room>(context), IRoomRepository
{
    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await Context.Set<Room>().AnyAsync(r => r.Name == name);
    }

    public async Task<Room?> FindByIdWithDetailsAsync(int roomId)
    {
        return await Context.Set<Room>()
            .Include(r => r.Inspections)
            .Include(r => r.Blocks)
            .FirstOrDefaultAsync(r => r.Id == roomId);
    }

    // Override to include details by default when fetching a single room
    public override async Task<Room?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        // TODO(rooms-result-pattern): pass cancellationToken once FindByIdWithDetailsAsync accepts it.
        return await FindByIdWithDetailsAsync(id);
    }

    // Override to include details when listing all rooms (useful for the UI)
    public override async Task<IEnumerable<Room>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Room>()
            .Include(r => r.Inspections)
            .Include(r => r.Blocks)
            .ToListAsync(cancellationToken);
    }
}