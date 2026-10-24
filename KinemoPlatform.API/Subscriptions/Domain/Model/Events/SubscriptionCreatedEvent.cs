using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Subscriptions.Domain.Model.Events;

public record SubscriptionCreatedEvent(int SubscriptionId, int CinemaId, int PlanId) : IEvent;