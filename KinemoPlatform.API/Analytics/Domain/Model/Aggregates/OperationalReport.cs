using KinemoPlatform.API.Analytics.Domain.Model.Commands;
using KinemoPlatform.API.Analytics.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Analytics.Domain.Model.Aggregates;

/// <summary>
///     Operational Report aggregate root.
/// </summary>
public partial class OperationalReport
{
    public OperationalReport()
    {
    }

    public OperationalReport(GenerateReportCommand command) : this()
    {
        ReportType = command.ReportType;
        PeriodStart = command.PeriodStart;
        PeriodEnd = command.PeriodEnd;
        GeneratedAt = DateTimeOffset.UtcNow;
        ExportFormat = command.Format;
    }

    public int Id { get; }
    public EReportType ReportType { get; private set; }
    public DateTimeOffset GeneratedAt { get; private set; }
    public DateTimeOffset PeriodStart { get; private set; }
    public DateTimeOffset PeriodEnd { get; private set; }
    public EExportFormat ExportFormat { get; private set; }
}