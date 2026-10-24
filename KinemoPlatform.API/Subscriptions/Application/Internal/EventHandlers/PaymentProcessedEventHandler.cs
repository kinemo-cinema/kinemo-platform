using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;
using KinemoPlatform.API.Subscriptions.Domain.Model.Events;

namespace KinemoPlatform.API.Subscriptions.Application.Internal.EventHandlers;

public class PaymentProcessedEventHandler : IEventHandler<PaymentProcessedEvent>
{
    public Task Handle(PaymentProcessedEvent domainEvent, CancellationToken cancellationToken)
    {
        if (domainEvent.Success)
        {
            Console.WriteLine("PAYMENT SUCCESSFUL: Subscription {0} extended for ${1}", domainEvent.SubscriptionId, domainEvent.Amount);
        }
        else
        {
            Console.WriteLine("PAYMENT FAILED: Subscription {0} is now Past Due.", domainEvent.SubscriptionId);
        }
        return Task.CompletedTask;
    }
}