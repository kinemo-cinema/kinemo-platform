using System.ComponentModel.DataAnnotations;
using KinemoPlatform.API.Analytics.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Analytics.Interfaces.REST.Resources;

public record GenerateReportResource(
    [EnumDataType(typeof(EReportType))]
    EReportType ReportType,

    DateTimeOffset PeriodStart,

    DateTimeOffset PeriodEnd,

    [EnumDataType(typeof(EExportFormat))]
    EExportFormat Format
);