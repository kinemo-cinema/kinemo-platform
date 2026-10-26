using KinemoPlatform.API.Analytics.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Analytics.Application.Internal.EventHandlers;

public class ReportGeneratedEventHandler : IEventHandler<ReportGeneratedEvent>
{
    public Task Handle(ReportGeneratedEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine("REPORT GENERATED: ID {0}, Type {1}, Format {2}", 
            domainEvent.ReportId, domainEvent.ReportType, domainEvent.Format);
        return Task.CompletedTask;
    }
}