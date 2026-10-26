using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Model.Commands;

namespace KinemoPlatform.API.Subscriptions.Domain.Services;

public interface ISubscriptionCommandService
{
    /// <summary>
    ///     Handles creating a new subscription (US39).
    /// </summary>
    Task<Subscription?> Handle(CreateSubscriptionCommand command);

    /// <summary>
    ///     Handles processing a payment for a subscription (US40).
    /// </summary>
    Task<Subscription?> Handle(ProcessPaymentCommand command);

    /// <summary>
    ///     Handles changing the plan of an existing subscription (US42).
    /// </summary>
    Task<Subscription?> Handle(ChangePlanCommand command);
}