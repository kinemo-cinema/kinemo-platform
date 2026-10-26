using KinemoPlatform.API.Analytics.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Analytics.Domain.Model.Entities;

public class OperationalMetric
{
    public OperationalMetric()
    {
    }

    public OperationalMetric(EMetricType metricType, decimal metricValue, DateTimeOffset periodStart, DateTimeOffset periodEnd) : this()
    {
        MetricType = metricType;
        MetricValue = metricValue;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
    }

    public int Id { get; }
    public int DashboardId { get; private set; }
    public EMetricType MetricType { get; private set; }
    public decimal MetricValue { get; private set; }
    public DateTimeOffset PeriodStart { get; private set; }
    public DateTimeOffset PeriodEnd { get; private set; }
}