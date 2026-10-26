namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;
public record SubscriptionUpgradeResource(int Id, int OldPlanId, int NewPlanId, DateTimeOffset ChangedAt);