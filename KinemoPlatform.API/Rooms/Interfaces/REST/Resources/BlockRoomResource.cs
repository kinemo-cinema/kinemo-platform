namespace KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

public record BlockRoomResource(string Reason, DateTimeOffset StartDateTime, DateTimeOffset EndDateTime);