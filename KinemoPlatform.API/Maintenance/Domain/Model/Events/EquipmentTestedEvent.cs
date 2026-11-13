using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;
using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Events;

public record EquipmentTestedEvent(int EquipmentId, int TestId, EEffectChannel Channel, bool IsSuccessful) : IEvent;