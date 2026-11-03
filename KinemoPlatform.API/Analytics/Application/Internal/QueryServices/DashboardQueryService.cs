using KinemoPlatform.API.Analytics.Application.Services;
using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Queries;
using KinemoPlatform.API.Analytics.Domain.Repositories;

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