using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Seats.Infrastructure.Persistence.EFC.Repositories;

public class SeatAllocationRepository(AppDbContext context) 
    : BaseRepository<SeatAllocation>(context), ISeatAllocationRepository
{
    public async Task<bool> ExistsByShowIdAsync(int showId)
    {
        return await Context.Set<SeatAllocation>().AnyAsync(a => a.ShowId == showId);
    }

    public async Task<SeatAllocation?> FindByShowIdWithDetailsAsync(int showId)
    {
        return await Context.Set<SeatAllocation>()
            .Include(a => a.ShowSeats)
            .Include(a => a.Interventions)
            .FirstOrDefaultAsync(a => a.ShowId == showId);
    }

    // Override standard FindByIdAsync to include the relationships
    public override async Task<SeatAllocation?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<SeatAllocation>()
            .Include(a => a.ShowSeats)
            .Include(a => a.Interventions)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }
}