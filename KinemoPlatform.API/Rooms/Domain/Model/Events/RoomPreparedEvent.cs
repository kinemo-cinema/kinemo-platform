using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Rooms.Domain.Model.Events;

public record RoomPreparedEvent(int RoomId, int InspectionId) : IEvent;