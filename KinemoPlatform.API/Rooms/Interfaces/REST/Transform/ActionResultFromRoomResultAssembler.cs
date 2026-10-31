using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Rooms.Application.Errors;
using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Transform;

/// <summary>
///     Maps the <see cref="Result{TValue,TError}" /> of a room command to an HTTP response.
/// </summary>
/// <remarks>
///     Failures are returned as RFC 7807 <c>application/problem+json</c> with a localized title.
/// </remarks>
public static class ActionResultFromRoomResultAssembler
{
    /// <summary>
    ///     Converts a command result into an <see cref="IActionResult" />.
    /// </summary>
    /// <param name="result">The result returned by the room command service.</param>
    /// <param name="controller">The controller used to build the problem responses.</param>
    /// <param name="localizer">Localizer used for the problem titles.</param>
    /// <param name="onSuccess">Builds the success response (200 OK, 201 Created, ...) from the room.</param>
    /// <returns>The HTTP response for the result.</returns>
    public static IActionResult ToActionResult(
        Result<Room, ERoomError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        Func<Room, IActionResult> onSuccess)
    {
        return result switch
        {
            Result<Room, ERoomError>.Success success => onSuccess(success.Value),
            Result<Room, ERoomError>.Failure failure => failure.Error switch
            {
                ERoomError.RoomNotFound => controller.Problem(
                    title: localizer["RoomNotFound"].Value,
                    statusCode: StatusCodes.Status404NotFound),
                ERoomError.DuplicateName => controller.Problem(
                    title: localizer["RoomNameDuplicated"].Value,
                    statusCode: StatusCodes.Status409Conflict),
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