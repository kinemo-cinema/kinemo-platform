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

    public void UpdateTimeSlot(DateTimeOffset startDateTime, DateTimeOffset endDateTime)
    {
        if (Status != EShowStatus.Scheduled)
            throw new InvalidOperationException("Only scheduled shows can be edited.");
        TimeSlot = new TimeSlot(startDateTime, endDateTime);
    }

    public void Cancel()
    {
        if (Status == EShowStatus.Finished)
            throw new InvalidOperationException("A finished show cannot be cancelled.");
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