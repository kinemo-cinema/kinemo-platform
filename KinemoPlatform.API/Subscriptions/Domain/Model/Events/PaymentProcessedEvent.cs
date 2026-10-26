using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Subscriptions.Domain.Model.Events;

public record PaymentProcessedEvent(int SubscriptionId, decimal Amount, bool Success) : IEvent;