using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Analytics.Interfaces.REST.Transform;
public static class DashboardResourceFromEntityAssembler
{
    public static DashboardResource ToResourceFromEntity(Dashboard e) => 
        new(e.Id, e.GeneratedAt, e.PeriodStart, e.PeriodEnd,
            e.Metrics.Select(OperationalMetricResourceFromEntityAssembler.ToResourceFromEntity),
            e.RoomPerformances.Select(RoomPerformanceResourceFromEntityAssembler.ToResourceFromEntity),
            e.IncidentSummaries.Select(IncidentSummaryResourceFromEntityAssembler.ToResourceFromEntity));
}