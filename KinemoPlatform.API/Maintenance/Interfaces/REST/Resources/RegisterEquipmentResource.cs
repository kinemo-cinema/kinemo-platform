using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;

public record RegisterEquipmentResource(int RoomId, string Name, EEquipmentType EquipmentType, int UsefulLifeHours);