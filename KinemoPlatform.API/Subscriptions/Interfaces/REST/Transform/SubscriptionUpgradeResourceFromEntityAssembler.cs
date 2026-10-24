using KinemoPlatform.API.Subscriptions.Domain.Model.Entities;
using KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Transform;
public static class SubscriptionUpgradeResourceFromEntityAssembler
{
    public static SubscriptionUpgradeResource ToResourceFromEntity(SubscriptionUpgrade e) => 
        new(e.Id, e.OldPlanId, e.NewPlanId, e.ChangedAt);
}