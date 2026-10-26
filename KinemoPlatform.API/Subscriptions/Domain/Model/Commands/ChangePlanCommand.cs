namespace KinemoPlatform.API.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to upgrade or downgrade a subscription to a new plan.
/// </summary>
public record ChangePlanCommand(int SubscriptionId, int NewPlanId);