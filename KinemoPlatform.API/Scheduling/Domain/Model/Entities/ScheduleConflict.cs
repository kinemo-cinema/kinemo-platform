namespace KinemoPlatform.API.Scheduling.Domain.Model.Entities;

/// <summary>
///     Represents a conflict detected between two overlapping shows.
/// </summary>
public class ScheduleConflict
{
    /// <summary>
    ///     Default constructor for the schedule conflict entity.
    /// </summary>
    public ScheduleConflict()
    {
        Description = string.Empty;
    }

    /// <summary>
    ///     Constructor for the schedule conflict entity.
    /// </summary>
    /// <param name="showId">The ID of the show that triggered the conflict.</param>
    /// <param name="conflictingShowId">The ID of the show it overlaps with.</param>
    /// <param name="description">A description of the conflict.</param>
    public ScheduleConflict(int showId, int conflictingShowId, string description) : this()
    {
        ShowId = showId;
        ConflictingShowId = conflictingShowId;
        Description = description;
        DetectedAt = DateTimeOffset.UtcNow;
        Resolved = false;
    }

    public int Id { get; }
    public int ShowId { get; private set; }
    public int ConflictingShowId { get; private set; }
    public DateTimeOffset DetectedAt { get; private set; }
    public string Description { get; private set; }
    public bool Resolved { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }

    /// <summary>
    ///     Marks the conflict as resolved.
    /// </summary>
    public void MarkAsResolved()
    {
        Resolved = true;
        ResolvedAt = DateTimeOffset.UtcNow;
    }
}