namespace KinemoPlatform.API.Scheduling.Interfaces.REST.Resources;

public record ScheduleShowResource(int MovieId, int RoomId, DateTimeOffset StartDateTime, DateTimeOffset EndDateTime);