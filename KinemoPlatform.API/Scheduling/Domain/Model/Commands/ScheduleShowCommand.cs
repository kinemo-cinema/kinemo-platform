namespace KinemoPlatform.API.Scheduling.Domain.Model.Commands;

/// <summary>
///     Command to schedule a new show.
/// </summary>
/// <param name="MovieId">
///     The unique identifier of the movie to be shown.
/// </param>
/// <param name="RoomId">
///     The unique identifier of the room where the show will take place.
/// </param>
/// <param name="StartDateTime">
///     The start date and time of the show.
/// </param>
/// <param name="EndDateTime">
///     The end date and time of the show.
/// </param>
public record ScheduleShowCommand(int MovieId, int RoomId, DateTimeOffset StartDateTime, DateTimeOffset EndDateTime);