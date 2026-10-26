using KinemoPlatform.API.Analytics.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
public record OperationalMetricResource(int Id, EMetricType MetricType, decimal MetricValue, DateTimeOffset PeriodStart, DateTimeOffset PeriodEnd);