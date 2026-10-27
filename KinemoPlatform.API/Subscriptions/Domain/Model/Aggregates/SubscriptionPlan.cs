using KinemoPlatform.API.Subscriptions.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;

/// <summary>
///     Represents a subscription tier (Starter, Growth, Enterprise).
/// </summary>
public partial class SubscriptionPlan
{
    public SubscriptionPlan()
    {
        Name = string.Empty;
    }

    public SubscriptionPlan(EPlanType planType, string name, decimal price, int maxScreens) : this()
    {
        PlanType = planType;
        Name = name;
        Price = price;
        MaxScreens = maxScreens;
    }

    public int Id { get; }
    public EPlanType PlanType { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int MaxScreens { get; private set; }
}