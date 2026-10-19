using KinemoPlatform.API.Scheduling.Domain.Model.Commands;
using KinemoPlatform.API.Scheduling.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;

/// <summary>
///     Show aggregate root entity in the Kinemo Platform.
/// </summary>
/// <remarks>
///     This class represents a scheduled 4D show. It encapsulates the
///     movie, the room, the time slot, and the lifecycle status.
/// </remarks>
public partial class Show
{
    /// <summary>
    ///     Default constructor for the show entity.
    /// </summary>
    public Show()
    {
        // Default placeholder slot
        TimeSlot = new TimeSlot(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
        Status = EShowStatus.Scheduled;
    }

    /// <summary>
    ///     Constructor for the show entity from a schedule command.
    /// </summary>
    /// <param name="command">
    ///     The <see cref="ScheduleShowCommand" /> command.
    /// </param>
    public Show(ScheduleShowCommand command) : this()
    {
        MovieId = command.MovieId;
        RoomId = command.RoomId;
        TimeSlot = new TimeSlot(command.StartDateTime, command.EndDateTime);
        Status = EShowStatus.Scheduled;
    }

    public int Id { get; }
    public int MovieId { get; private set; }
    public int RoomId { get; private set; }
    
    /// <summary>
    ///     The time slot for this show (start and end times).
    /// </summary>
    public TimeSlot TimeSlot { get; private set; }
    
    public EShowStatus Status { get; private set; }

    /// <summary>
    ///     Update the time slot of an existing show.
    /// </summary>
    /// <param name="startDateTime">The new start time.</param>
    /// <param name="endDateTime">The new end time.</param>
    public void UpdateTimeSlot(DateTimeOffset startDateTime, DateTimeOffset endDateTime)
    {
        TimeSlot = new TimeSlot(startDateTime, endDateTime);
    }

    /// <summary>
    ///     Mark the show as cancelled.
    /// </summary>
    public void Cancel()
    {
        Status = EShowStatus.Cancelled;
    }

    /// <summary>
    ///     Mark the show as finished.
    /// </summary>
    public void Finish()
    {
        Status = EShowStatus.Finished;
    }
}