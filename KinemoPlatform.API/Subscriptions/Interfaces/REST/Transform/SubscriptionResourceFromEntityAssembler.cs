using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Transform;
public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResourceFromEntity(Subscription e) => 
        new(e.Id, e.CinemaId, e.PlanId, e.Status, e.StartDate, e.EndDate,
            e.Transactions.Select(PaymentTransactionResourceFromEntityAssembler.ToResourceFromEntity),
            e.Upgrades.Select(SubscriptionUpgradeResourceFromEntityAssembler.ToResourceFromEntity));
}