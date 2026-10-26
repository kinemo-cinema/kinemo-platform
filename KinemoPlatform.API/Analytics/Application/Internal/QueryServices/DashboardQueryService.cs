using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Queries;
using KinemoPlatform.API.Analytics.Domain.Repositories;
using KinemoPlatform.API.Analytics.Domain.Services;

namespace KinemoPlatform.API.Analytics.Application.Internal.QueryServices;

public class DashboardQueryService(IDashboardRepository dashboardRepository) : IDashboardQueryService
{
    public async Task<Dashboard?> Handle(GetDashboardByIdQuery query)
    {
        return await dashboardRepository.FindByIdWithDetailsAsync(query.DashboardId);
    }

    public async Task<Dashboard?> Handle(GetLatestDashboardQuery query)
    {
        return await dashboardRepository.FindLatestAsync();
    }
}