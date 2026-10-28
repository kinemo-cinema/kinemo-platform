using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Commands;

public record RunEquipmentTestCommand(int EquipmentId, EEffectChannel Channel);