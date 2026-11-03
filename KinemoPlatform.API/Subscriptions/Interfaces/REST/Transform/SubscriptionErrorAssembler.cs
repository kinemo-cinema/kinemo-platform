using KinemoPlatform.API.Subscriptions.Application.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Transform;

public static class SubscriptionErrorAssembler
{
    public static ProblemDetails ToProblemDetails(
        ESubscriptionError error)
    {
        return error switch
        {
            ESubscriptionError.PlanNotFound => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Subscription plan not found",
                Detail = "The requested subscription plan does not exist."
            },

            ESubscriptionError.SubscriptionNotFound => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Subscription not found",
                Detail = "The requested subscription does not exist."
            },

            ESubscriptionError.InvalidPayment => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid payment",
                Detail = "The payment information is invalid."
            },

            ESubscriptionError.InvalidPlanChange => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Invalid plan change",
                Detail = "The requested plan change cannot be completed."
            },

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Unexpected subscription error"
            }
        };
    }
}