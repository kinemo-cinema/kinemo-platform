using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Scheduling.Application.Errors;
using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Scheduling.Interfaces.REST.Transform;

/// <summary>
///     Maps the <see cref="Result{TValue,TError}" /> of a show command to an HTTP response.
/// </summary>
/// <remarks>
///     Failures are returned as RFC 7807 <c>application/problem+json</c> with a localized title.
///     A request that is well formed but collides with another show (room already booked, overlapping
///     time slot) is a 409 Conflict; 400 is reserved for invalid requests.
/// </remarks>
public static class ActionResultFromShowResultAssembler
{
    /// <summary>
    ///     Converts a command result into an <see cref="IActionResult" />.
    /// </summary>
    /// <param name="result">The result returned by the show command service.</param>
    /// <param name="controller">The controller used to build the problem responses.</param>
    /// <param name="localizer">Localizer used for the problem titles.</param>
    /// <param name="onSuccess">Builds the success response (200 OK, 201 Created, ...) from the show.</param>
    /// <returns>The HTTP response for the result.</returns>
    public static IActionResult ToActionResult(
        Result<Show, EShowError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        Func<Show, IActionResult> onSuccess)
    {
        return result switch
        {
            Result<Show, EShowError>.Success success => onSuccess(success.Value),
            Result<Show, EShowError>.Failure failure => failure.Error switch
            {
                EShowError.ShowNotFound => controller.Problem(
                    title: localizer["ShowNotFound"].Value,
                    statusCode: StatusCodes.Status404NotFound),
                EShowError.RoomAlreadyBooked => controller.Problem(
                    title: localizer["ShowRoomAlreadyBooked"].Value,
                    statusCode: StatusCodes.Status409Conflict),
                EShowError.TimeSlotOverlap => controller.Problem(
                    title: localizer["ShowTimeSlotOverlaps"].Value,
                    statusCode: StatusCodes.Status409Conflict),
                EShowError.InvalidTimeSlot => controller.Problem(
                    title: localizer["ShowInvalidTimeSlot"].Value,
                    statusCode: StatusCodes.Status400BadRequest),
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