using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Commands;

public record RegisterEquipmentCommand(int RoomId, string Name, EEquipmentType EquipmentType, int UsefulLifeHours);