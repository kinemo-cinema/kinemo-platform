namespace KinemoPlatform.API.Rooms.Domain.Model.Commands;
public record CreateRoomBlockCommand(int RoomId, string Reason, DateTimeOffset StartDateTime, DateTimeOffset EndDateTime);