using KinemoPlatform.API.Scheduling.Application.Services;
using KinemoPlatform.API.Scheduling.Domain.Model.Queries;
using KinemoPlatform.API.Scheduling.Domain.Model.ValueObjects;
using KinemoPlatform.API.Scheduling.Interfaces.ACL;

namespace KinemoPlatform.API.Scheduling.Application.ACL;

public class SchedulingContextFacade(IShowQueryService showQueryService) : ISchedulingContextFacade
{
    public async Task<IReadOnlyList<ShowSlot>> FetchActiveShowSlots(
        DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default)
    {
        var shows = await showQueryService.Handle(new GetAllShowsQuery(), cancellationToken);
        return shows
            .Where(show => show.Status != EShowStatus.Cancelled &&
                           show.TimeSlot.StartDateTime < to &&
                           show.TimeSlot.EndDateTime > from)
            .Select(show => new ShowSlot(show.Id, show.RoomId,
                show.TimeSlot.StartDateTime, show.TimeSlot.EndDateTime))
            .ToList();
    }
    
    public async Task<ShowInfo?> FetchShowInfo(int showId, CancellationToken cancellationToken = default)
    {
        var show = await showQueryService.Handle(new GetShowByIdQuery(showId), cancellationToken);
        return show is null ? null : new ShowInfo(show.Id, show.RoomId, show.Status == EShowStatus.Scheduled);
    }
}