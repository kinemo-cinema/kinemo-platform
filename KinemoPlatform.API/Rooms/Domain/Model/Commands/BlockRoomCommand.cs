namespace KinemoPlatform.API.Rooms.Domain.Model.Commands;

public record BlockRoomCommand(int RoomId, string Reason, DateTimeOffset StartDateTime, DateTimeOffset EndDateTime);