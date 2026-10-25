namespace KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
public record DashboardResource(
    int Id,
    DateTimeOffset GeneratedAt,
    DateTimeOffset PeriodStart,
    DateTimeOffset PeriodEnd,
    IEnumerable<OperationalMetricResource> Metrics,
    IEnumerable<RoomPerformanceResource> RoomPerformances,
    IEnumerable<IncidentSummaryResource> IncidentSummaries);