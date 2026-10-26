using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Rooms.Domain.Model.Events;

public record RoomBlockedEvent(int RoomId, int BlockId, string Reason) : IEvent;