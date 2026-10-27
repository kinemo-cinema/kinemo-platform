using KinemoPlatform.API.Analytics.Domain.Model.Entities;
using KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Analytics.Interfaces.REST.Transform;
public static class OperationalMetricResourceFromEntityAssembler
{
    public static OperationalMetricResource ToResourceFromEntity(OperationalMetric e) => 
        new(e.Id, e.MetricType, e.MetricValue, e.PeriodStart, e.PeriodEnd);
}