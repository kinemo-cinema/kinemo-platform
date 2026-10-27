using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;
using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Events;

public record EquipmentRegisteredEvent(int EquipmentId, int RoomId, EEquipmentType Type) : IEvent;