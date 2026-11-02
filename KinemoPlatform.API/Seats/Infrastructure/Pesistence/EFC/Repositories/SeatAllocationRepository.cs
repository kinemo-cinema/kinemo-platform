using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Seats.Infrastructure.Persistence.EFC.Repositories;

public class SeatAllocationRepository(AppDbContext context) 
    : BaseRepository<SeatAllocation>(context), ISeatAllocationRepository
{
    public async Task<bool> ExistsByShowIdAsync(int showId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<SeatAllocation>().AnyAsync(a => a.ShowId == showId, cancellationToken);
    }

    public async Task<SeatAllocation?> FindByShowIdWithDetailsAsync(int showId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<SeatAllocation>()
            .Include(a => a.ShowSeats)
            .Include(a => a.Interventions)
            .FirstOrDefaultAsync(a => a.ShowId == showId, cancellationToken);
    }

    // Override standard FindByIdAsync to include the relationships
    public new async Task<SeatAllocation?> FindByIdAsync(int id)
    {
        return await Context.Set<SeatAllocation>()
            .Include(a => a.ShowSeats)
            .Include(a => a.Interventions)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}