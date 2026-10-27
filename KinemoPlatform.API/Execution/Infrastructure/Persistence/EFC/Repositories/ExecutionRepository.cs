using KinemoPlatform.API.Execution.Domain.Model.Aggregates;
using KinemoPlatform.API.Execution.Domain.Model.ValueObjects;
using KinemoPlatform.API.Execution.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Execution.Infrastructure.Persistence.EFC.Repositories;

public class ExecutionRepository(AppDbContext context) 
    : BaseRepository<ShowExecution>(context), IExecutionRepository
{
    public async Task<ShowExecution?> FindByIdWithDetailsAsync(int executionId)
    {
        return await Context.Set<ShowExecution>()
            .Include(e => e.Sequences)
            .Include(e => e.SyncEvents)
            .Include(e => e.HardwareLogs)
            .Include(e => e.EmergencyEvents)
            .FirstOrDefaultAsync(e => e.Id == executionId);
    }

    public async Task<ShowExecution?> FindActiveByShowIdAsync(int showId)
    {
        return await Context.Set<ShowExecution>()
            .Include(e => e.Sequences)
            .Include(e => e.SyncEvents)
            .Include(e => e.HardwareLogs)
            .Include(e => e.EmergencyEvents)
            .FirstOrDefaultAsync(e => e.ShowId == showId && 
                                      (e.Status == EExecutionStatus.InProgress || e.Status == EExecutionStatus.Paused));
    }
}