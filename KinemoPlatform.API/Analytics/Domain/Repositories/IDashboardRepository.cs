using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Analytics.Domain.Repositories;

/// <summary>
///     Represents the dashboard repository in the Kinemo Platform.
/// </summary>
public interface IDashboardRepository : IBaseRepository<Dashboard>
{
    /// <summary>
    ///     Retrieves a dashboard by ID, eagerly loading metrics, room performances, and incident summaries.
    /// </summary>
    Task<Dashboard?> FindByIdWithDetailsAsync(int dashboardId);

    /// <summary>
    ///     Retrieves the most recently generated dashboard.
    /// </summary>
    Task<Dashboard?> FindLatestAsync();
}