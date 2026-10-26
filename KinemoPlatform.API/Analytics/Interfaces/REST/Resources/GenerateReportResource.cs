using KinemoPlatform.API.Analytics.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
public record GenerateReportResource(EReportType ReportType, DateTimeOffset PeriodStart, DateTimeOffset PeriodEnd, EExportFormat Format);