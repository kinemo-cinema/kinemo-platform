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
        int? excludeShowId = null,
        CancellationToken cancellationToken = default)
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
            endDateTime > s.TimeSlot.StartDateTime,
            cancellationToken);
    }

    public async Task<IEnumerable<Show>> FindByDateAsync(DateTimeOffset date, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Show>()
            .Where(s => s.TimeSlot.StartDateTime.Date == date.Date)
            .OrderBy(s => s.TimeSlot.StartDateTime)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Show>> FindByFilterAsync(
        DateTimeOffset? date, int? roomId, int? movieId, EShowStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<Show>().AsNoTracking().AsQueryable();

        // Same date expression as FindByDateAsync, so the filter translates exactly like the daily schedule did.
        if (date is not null)
        {
            var day = date.Value.Date;
            query = query.Where(s => s.TimeSlot.StartDateTime.Date == day);
        }
        if (roomId is not null) query = query.Where(s => s.RoomId == roomId.Value);
        if (movieId is not null) query = query.Where(s => s.MovieId == movieId.Value);
        if (status is not null) query = query.Where(s => s.Status == status.Value);

        // The daily schedule is ordered by start time; the plain list keeps the previous (id) order.
        query = date is not null
            ? query.OrderBy(s => s.TimeSlot.StartDateTime)
            : query.OrderBy(s => s.Id);

        return await query.ToListAsync(cancellationToken);
    }
}