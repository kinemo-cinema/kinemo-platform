using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Ticketing.Application.Errors;
using KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Transform;

/// <summary>
///     Maps the <see cref="Result{TValue,TError}" /> of a ticketing command to an HTTP response.
/// </summary>
/// <remarks>
///     Failures are returned as RFC 7807 <c>application/problem+json</c> with a localized title.
/// </remarks>
public static class ActionResultFromTicketingResultAssembler
{
    /// <summary>
    ///     Converts a command result into an <see cref="IActionResult" />.
    /// </summary>
    /// <param name="result">The result returned by the ticketing command service.</param>
    /// <param name="controller">The controller used to build the problem responses.</param>
    /// <param name="localizer">Localizer used for the problem titles.</param>
    /// <param name="onSuccess">Builds the success response (200 OK, 201 Created, ...) from the integration.</param>
    /// <returns>The HTTP response for the result.</returns>
    public static IActionResult ToActionResult(
        Result<TicketingIntegration, ETicketingError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        Func<TicketingIntegration, IActionResult> onSuccess)
    {
        return result switch
        {
            Result<TicketingIntegration, ETicketingError>.Success success => onSuccess(success.Value),
            Result<TicketingIntegration, ETicketingError>.Failure failure => failure.Error switch
            {
                ETicketingError.IntegrationNotFound => controller.Problem(
                    title: localizer["TicketingIntegrationNotFound"].Value,
                    statusCode: StatusCodes.Status404NotFound),
                _ => controller.Problem(
                    title: localizer["UnexpectedServerError"].Value,
                    statusCode: StatusCodes.Status500InternalServerError)
            },
            _ => controller.Problem(
                title: localizer["UnexpectedServerError"].Value,
                statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}