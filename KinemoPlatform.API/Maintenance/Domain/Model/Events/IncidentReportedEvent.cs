using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;
using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Events;

public record IncidentReportedEvent(int EquipmentId, int IncidentId, EIncidentSeverity Severity) : IEvent;