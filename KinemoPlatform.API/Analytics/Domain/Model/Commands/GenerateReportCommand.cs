using KinemoPlatform.API.Analytics.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Analytics.Domain.Model.Commands;

/// <summary>
///     Command to generate an operational report.
/// </summary>
public record GenerateReportCommand(EReportType ReportType, DateTimeOffset PeriodStart, DateTimeOffset PeriodEnd, EExportFormat Format);