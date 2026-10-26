using KinemoPlatform.API.Subscriptions.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;
public record SubscriptionResource(
    int Id,
    int CinemaId,
    int PlanId,
    ESubscriptionStatus Status,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    IEnumerable<PaymentTransactionResource> Transactions,
    IEnumerable<SubscriptionUpgradeResource> Upgrades);