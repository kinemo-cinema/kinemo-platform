namespace KinemoPlatform.API.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to process the payment for a subscription.
/// </summary>
/// <param name="SubscriptionId">The ID of the subscription being paid.</param>
/// <param name="Amount">The amount to charge.</param>
/// <param name="PaymentToken">A token from the payment gateway (e.g., Stripe).</param>
public record ProcessPaymentCommand(int SubscriptionId, decimal Amount, string PaymentToken);