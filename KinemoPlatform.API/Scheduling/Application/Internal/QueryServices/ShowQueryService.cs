using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Scheduling.Domain.Model.Queries;
using KinemoPlatform.API.Scheduling.Domain.Repositories;
using KinemoPlatform.API.Scheduling.Domain.Services;

namespace KinemoPlatform.API.Scheduling.Application.Internal.QueryServices;

public class ShowQueryService(IShowRepository showRepository) : IShowQueryService
{
    public async Task<Show?> Handle(GetShowByIdQuery query)
    {
        return await showRepository.FindByIdAsync(query.ShowId);
    }

    public async Task<IEnumerable<Show>> Handle(GetAllShowsQuery query)
    {
        return await showRepository.ListAsync();
    }

    public async Task<IEnumerable<Show>> Handle(GetDailyScheduleQuery query)
    {
        return await showRepository.FindByDateAsync(query.Date);
    }
}