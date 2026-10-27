using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Scheduling.Domain.Model.Queries;

namespace KinemoPlatform.API.Scheduling.Domain.Services;

/// <summary>
///     Represents the show query service in the Kinemo Platform.
/// </summary>
public interface IShowQueryService
{
    /// <summary>
    ///     Handles the retrieval of a show by its ID.
    /// </summary>
    Task<Show?> Handle(GetShowByIdQuery query);

    /// <summary>
    ///     Handles the retrieval of all shows.
    /// </summary>
    Task<IEnumerable<Show>> Handle(GetAllShowsQuery query);

    /// <summary>
    ///     Handles the retrieval of the daily schedule (US09).
    /// </summary>
    Task<IEnumerable<Show>> Handle(GetDailyScheduleQuery query);
}