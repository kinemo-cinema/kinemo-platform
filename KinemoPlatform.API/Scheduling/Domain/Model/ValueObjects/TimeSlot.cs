namespace KinemoPlatform.API.Scheduling.Domain.Model.ValueObjects;

/// <summary>
///     Represents a continuous block of time for a scheduled show.
/// </summary>
/// <param name="StartDateTime">
///     The exact date and time when the show starts.
/// </param>
/// <param name="EndDateTime">
///     The exact date and time when the show ends.
/// </param>
public record TimeSlot(DateTimeOffset StartDateTime, DateTimeOffset EndDateTime)
{
    /// <summary>
    ///     Validates that the time slot is logical (end time is after start time).
    /// </summary>
    /// <returns>True if valid, false otherwise.</returns>
    public bool IsValid() => EndDateTime > StartDateTime;

    /// <summary>
    ///     Checks if this time slot overlaps with another time slot.
    /// </summary>
    /// <param name="other">The other time slot to check against.</param>
    /// <returns>True if they overlap, false otherwise.</returns>
    public bool OverlapsWith(TimeSlot other)
    {
        return StartDateTime < other.EndDateTime && EndDateTime > other.StartDateTime;
    }

    /// <summary>
    ///     Gets the duration of the time slot in minutes.
    /// </summary>
    public double DurationInMinutes => (EndDateTime - StartDateTime).TotalMinutes;
}