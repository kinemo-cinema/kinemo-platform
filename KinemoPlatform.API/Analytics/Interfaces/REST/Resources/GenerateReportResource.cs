using System.ComponentModel.DataAnnotations;
using KinemoPlatform.API.Analytics.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Analytics.Interfaces.REST.Resources;

public record GenerateReportResource(
    [property: EnumDataType(typeof(EReportType))]
    EReportType ReportType,

    DateTimeOffset PeriodStart,

    DateTimeOffset PeriodEnd,

    [property: EnumDataType(typeof(EExportFormat))]
    EExportFormat Format
);