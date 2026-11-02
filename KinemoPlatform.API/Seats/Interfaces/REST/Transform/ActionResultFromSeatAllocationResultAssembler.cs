using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Seats.Application.Errors;
using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Transform;

/// <summary>
///     Maps the <see cref="Result{TValue,TError}" /> of a seat allocation command to an HTTP response.
/// </summary>
/// <remarks>
///     Failures are returned as RFC 7807 <c>application/problem+json</c> with a localized title.
/// </remarks>
public static class ActionResultFromSeatAllocationResultAssembler
{
    /// <summary>
    ///     Converts a command result into an <see cref="IActionResult" />.
    /// </summary>
    /// <param name="result">The result returned by the seat allocation command service.</param>
    /// <param name="controller">The controller used to build the problem responses.</param>
    /// <param name="localizer">Localizer used for the problem titles.</param>
    /// <param name="onSuccess">Builds the success response (200 OK, 201 Created, ...) from the allocation.</param>
    /// <returns>The HTTP response for the result.</returns>
    public static IActionResult ToActionResult(
        Result<SeatAllocation, ESeatAllocationError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        Func<SeatAllocation, IActionResult> onSuccess)
    {
        return result switch
        {
            Result<SeatAllocation, ESeatAllocationError>.Success success => onSuccess(success.Value),
            Result<SeatAllocation, ESeatAllocationError>.Failure failure => failure.Error switch
            {
                ESeatAllocationError.AllocationAlreadyExists => controller.Problem(
                    title: localizer["SeatAllocationAlreadyExists"].Value,
                    statusCode: StatusCodes.Status409Conflict),
                ESeatAllocationError.AllocationNotFound => controller.Problem(
                    title: localizer["SeatAllocationNotFound"].Value,
                    statusCode: StatusCodes.Status404NotFound),
                ESeatAllocationError.SeatNotFound => controller.Problem(
                    title: localizer["SeatNotFound"].Value,
                    statusCode: StatusCodes.Status404NotFound),
                ESeatAllocationError.UnsupportedSeatStatus => controller.Problem(
                    title: localizer["SeatStatusNotSupported"].Value,
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
