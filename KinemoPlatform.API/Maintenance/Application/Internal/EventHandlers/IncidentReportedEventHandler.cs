using KinemoPlatform.API.Maintenance.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Maintenance.Application.Internal.EventHandlers;

public class IncidentReportedEventHandler : IEventHandler<IncidentReportedEvent>
{
    public Task Handle(IncidentReportedEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine("INCIDENT REPORTED: Equipment {0} (Severity: {1})", domainEvent.EquipmentId, domainEvent.Severity);
        return Task.CompletedTask;
    }
}