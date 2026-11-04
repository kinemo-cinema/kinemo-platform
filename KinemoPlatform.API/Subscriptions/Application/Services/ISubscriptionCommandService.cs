using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Subscriptions.Application.Errors;
using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Model.Commands;

namespace KinemoPlatform.API.Subscriptions.Application.Services;

public interface ISubscriptionCommandService
{
    /// <summary>
    ///     Handles creating a new subscription (US39).
    /// </summary>
    Task<Result<Subscription, ESubscriptionError>> Handle(
        CreateSubscriptionCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles processing a payment for a subscription (US40).
    /// </summary>
    Task<Result<Subscription, ESubscriptionError>> Handle(
        ProcessPaymentCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles changing the plan of an existing subscription (US42).
    /// </summary>
    Task<Result<Subscription, ESubscriptionError>> Handle(
        ChangePlanCommand command,
        CancellationToken cancellationToken = default);
}