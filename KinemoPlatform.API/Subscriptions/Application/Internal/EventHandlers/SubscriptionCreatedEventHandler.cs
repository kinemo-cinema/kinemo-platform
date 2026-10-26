using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;
using KinemoPlatform.API.Subscriptions.Domain.Model.Events;

namespace KinemoPlatform.API.Subscriptions.Application.Internal.EventHandlers;

public class SubscriptionCreatedEventHandler : IEventHandler<SubscriptionCreatedEvent>
{
    public Task Handle(SubscriptionCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            "SUBSCRIPTION CREATED: ID {0} for Cinema {1} on Plan {2}",
            domainEvent.SubscriptionId, domainEvent.CinemaId, domainEvent.PlanId);
        return Task.CompletedTask;
    }
}