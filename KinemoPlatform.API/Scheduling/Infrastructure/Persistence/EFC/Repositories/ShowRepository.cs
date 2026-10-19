using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Scheduling.Domain.Model.ValueObjects;
using KinemoPlatform.API.Scheduling.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Scheduling.Infrastructure.Persistence.EFC.Repositories;

public class ShowRepository(AppDbContext context) 
    : BaseRepository<Show>(context), IShowRepository
{
    public async Task<bool> ExistsOverlappingShowAsync(
        int roomId, 
        DateTimeOffset startDateTime, 
        DateTimeOffset endDateTime, 
        int? excludeShowId = null)
    {
        var query = Context.Set<Show>()
            .Where(s => s.RoomId == roomId)
            .Where(s => s.Status != EShowStatus.Cancelled); // Ignore cancelled shows

        if (excludeShowId.HasValue)
        {
            query = query.Where(s => s.Id != excludeShowId.Value);
        }

        // Overlap logic: New show starts before existing ends AND new show ends after existing starts
        return await query.AnyAsync(s => 
            startDateTime < s.TimeSlot.EndDateTime && 
            endDateTime > s.TimeSlot.StartDateTime);
    }

    public async Task<IEnumerable<Show>> FindByDateAsync(DateTimeOffset date)
    {
        return await Context.Set<Show>()
            .Where(s => s.TimeSlot.StartDateTime.Date == date.Date)
            .OrderBy(s => s.TimeSlot.StartDateTime)
            .ToListAsync();
    }
}