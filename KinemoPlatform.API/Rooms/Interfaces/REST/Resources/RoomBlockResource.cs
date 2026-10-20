namespace KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

public record RoomBlockResource(int Id, string Reason, DateTimeOffset StartDateTime, DateTimeOffset EndDateTime, bool Active);