using KinemoPlatform.API.Scheduling.Application.Services;
using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Scheduling.Domain.Model.Queries;
using KinemoPlatform.API.Scheduling.Domain.Repositories;

namespace KinemoPlatform.API.Scheduling.Application.Internal.QueryServices;

public class ShowQueryService(IShowRepository showRepository) : IShowQueryService
{
    public async Task<Show?> Handle(GetShowByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await showRepository.FindByIdAsync(query.ShowId, cancellationToken);
    }

    public async Task<IEnumerable<Show>> Handle(GetAllShowsQuery query, CancellationToken cancellationToken = default)
    {
        return await showRepository.ListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Show>> Handle(GetDailyScheduleQuery query, CancellationToken cancellationToken = default)
    {
        return await showRepository.FindByDateAsync(query.Date, cancellationToken);
    }
}