using KinemoPlatform.API.Analytics.Domain.Model.Commands;
using KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Analytics.Interfaces.REST.Transform;
public static class GenerateReportCommandFromResourceAssembler
{
    public static GenerateReportCommand ToCommandFromResource(GenerateReportResource r) => 
        new(r.ReportType, r.PeriodStart, r.PeriodEnd, r.Format);
}