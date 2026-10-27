using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;
using KinemoPlatform.API.Ticketing.Domain.Model.Events;

namespace KinemoPlatform.API.Ticketing.Application.Internal.EventHandlers;

public class SyncFailedEventHandler : IEventHandler<SyncFailedEvent>
{
    public Task Handle(SyncFailedEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            "SYNC FAILED: Integration {0} failed to sync Show {1}. Reason: {2}",
            domainEvent.IntegrationId, domainEvent.ShowId, domainEvent.ErrorDetails);
        return Task.CompletedTask;
    }
}