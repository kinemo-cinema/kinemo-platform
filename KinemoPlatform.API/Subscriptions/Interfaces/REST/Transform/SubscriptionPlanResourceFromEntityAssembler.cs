using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Transform;
public static class SubscriptionPlanResourceFromEntityAssembler
{
    public static SubscriptionPlanResource ToResourceFromEntity(SubscriptionPlan e) => 
        new(e.Id, e.PlanType, e.Name, e.Price, e.MaxScreens);
}