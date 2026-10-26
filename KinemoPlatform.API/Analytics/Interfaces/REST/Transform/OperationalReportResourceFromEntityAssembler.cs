using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Analytics.Interfaces.REST.Transform;
public static class OperationalReportResourceFromEntityAssembler
{
    public static OperationalReportResource ToResourceFromEntity(OperationalReport e) => 
        new(e.Id, e.ReportType, e.GeneratedAt, e.PeriodStart, e.PeriodEnd, e.ExportFormat);
}