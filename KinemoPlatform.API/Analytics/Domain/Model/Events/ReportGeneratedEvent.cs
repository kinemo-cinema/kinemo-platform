using KinemoPlatform.API.Analytics.Domain.Model.ValueObjects;
using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Analytics.Domain.Model.Events;

public record ReportGeneratedEvent(int ReportId, EReportType ReportType, EExportFormat Format) : IEvent;