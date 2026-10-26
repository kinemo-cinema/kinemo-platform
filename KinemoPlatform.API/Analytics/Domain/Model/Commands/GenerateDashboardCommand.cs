namespace KinemoPlatform.API.Analytics.Domain.Model.Commands;

/// <summary>
///     Command to generate a new operational dashboard for a specific time period.
/// </summary>
public record GenerateDashboardCommand(DateTimeOffset PeriodStart, DateTimeOffset PeriodEnd);