using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Queries;

namespace KinemoPlatform.API.Analytics.Domain.Services;

public interface IDashboardQueryService
{
    Task<Dashboard?> Handle(GetDashboardByIdQuery query);
    Task<Dashboard?> Handle(GetLatestDashboardQuery query);
}