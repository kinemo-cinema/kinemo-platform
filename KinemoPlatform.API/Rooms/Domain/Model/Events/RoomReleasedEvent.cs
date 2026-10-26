using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Rooms.Domain.Model.Events;

public record RoomReleasedEvent(int RoomId) : IEvent;