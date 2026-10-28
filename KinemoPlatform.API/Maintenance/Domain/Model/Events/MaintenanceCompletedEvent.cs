using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Events;

public record MaintenanceCompletedEvent(int MaintenanceOrderId, int EquipmentId) : IEvent;