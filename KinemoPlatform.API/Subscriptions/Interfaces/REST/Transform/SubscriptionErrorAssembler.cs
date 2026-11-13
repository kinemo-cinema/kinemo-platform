using KinemoPlatform.API.Subscriptions.Application.Errors;
using KinemoPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Transform;

public static class SubscriptionErrorAssembler
{
    public static ProblemDetails ToProblemDetails(
        ESubscriptionError error,
        IStringLocalizer<SharedResource> localizer)
    {
        var (status, key) = error switch
        {
            ESubscriptionError.PlanNotFound =>
                (StatusCodes.Status404NotFound, "Subscriptions.PlanNotFound"),
            ESubscriptionError.SubscriptionNotFound =>
                (StatusCodes.Status404NotFound, "Subscriptions.SubscriptionNotFound"),
            ESubscriptionError.PaymentProviderUnavailable =>
                (StatusCodes.Status503ServiceUnavailable, "Subscriptions.PaymentProviderUnavailable"),
            ESubscriptionError.InvalidPayment =>
                (StatusCodes.Status400BadRequest, "Subscriptions.InvalidPayment"),
            ESubscriptionError.InvalidPlanChange =>
                (StatusCodes.Status409Conflict, "Subscriptions.InvalidPlanChange"),
            _ => (StatusCodes.Status500InternalServerError, "")
        };

        return new ProblemDetails
        {
            Status = status,
            Title = string.IsNullOrEmpty(key)
                ? localizer["UnexpectedServerError"].Value
                : localizer[$"{key}.Title"].Value,
            Detail = string.IsNullOrEmpty(key)
                ? localizer["UnexpectedErrorProcessingRequest"].Value
                : localizer[$"{key}.Detail"].Value
        };
    }
}
