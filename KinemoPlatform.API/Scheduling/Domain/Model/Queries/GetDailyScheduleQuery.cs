namespace KinemoPlatform.API.Scheduling.Domain.Model.Queries;

/// <summary>
///     Query to retrieve the schedule for a specific day.
/// </summary>
/// <param name="Date">The date to retrieve shows for.</param>
public record GetDailyScheduleQuery(DateTimeOffset Date);