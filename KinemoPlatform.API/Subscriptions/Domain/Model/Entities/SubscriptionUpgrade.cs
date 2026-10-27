namespace KinemoPlatform.API.Subscriptions.Domain.Model.Entities;

public class SubscriptionUpgrade
{
    public SubscriptionUpgrade()
    {
    }

    public SubscriptionUpgrade(int oldPlanId, int newPlanId) : this()
    {
        OldPlanId = oldPlanId;
        NewPlanId = newPlanId;
        ChangedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; }
    public int SubscriptionId { get; private set; }
    public int OldPlanId { get; private set; }
    public int NewPlanId { get; private set; }
    public DateTimeOffset ChangedAt { get; private set; }
}