namespace KinemoPlatform.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
///     Represents the lifecycle status of a subscription contract.
/// </summary>
public enum ESubscriptionStatus
{
    Active,
    PastDue,
    Cancelled,
    Expired
}