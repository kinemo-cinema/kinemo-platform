using System.ComponentModel.DataAnnotations;

namespace KinemoPlatform.API.Scheduling.Interfaces.REST.Resources;

public record ScheduleShowResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidShowMovieId")]
    int MovieId,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidShowRoomId")]
    int RoomId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime);