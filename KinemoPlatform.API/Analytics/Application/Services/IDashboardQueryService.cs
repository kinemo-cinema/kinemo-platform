using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Queries;

namespace KinemoPlatform.API.Analytics.Application.Services;

public interface IDashboardQueryService
{
    Task<Dashboard?> Handle(
        GetDashboardByIdQuery query,
        CancellationToken cancellationToken = default);

    Task<Dashboard?> Handle(
        GetLatestDashboardQuery query,
        CancellationToken cancellationToken = default);
}