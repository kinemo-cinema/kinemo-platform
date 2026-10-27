using KinemoPlatform.API.Analytics.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
public record OperationalReportResource(
    int Id,
    EReportType ReportType,
    DateTimeOffset GeneratedAt,
    DateTimeOffset PeriodStart,
    DateTimeOffset PeriodEnd,
    EExportFormat ExportFormat);