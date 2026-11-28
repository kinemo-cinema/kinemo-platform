using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Rooms.Application.Errors;
using KinemoPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Transform;

public static class ActionResultFromRoomResultAssembler
{
    public static IActionResult ToActionResult<T>(
        Result<T, ERoomError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        Func<T, IActionResult> onSuccess)
    {
        return result switch
        {
            Result<T, ERoomError>.Success success => onSuccess(success.Value),
            Result<T, ERoomError>.Failure failure => failure.Error switch
            {
                ERoomError.RoomNotFound => controller.Problem(
                    title: localizer["RoomNotFound"].Value,
                    statusCode: StatusCodes.Status404NotFound),
                ERoomError.BlockNotFound => controller.Problem(
                    title: localizer["RoomBlockNotFound"].Value,
                    statusCode: StatusCodes.Status404NotFound),
                ERoomError.DuplicateName => controller.Problem(
                    title: localizer["RoomNameDuplicated"].Value,
                    statusCode: StatusCodes.Status409Conflict),
                ERoomError.RoomNotAvailableForPreparation => controller.Problem(
                    title: localizer["RoomNotAvailableForPreparation"].Value,
                    statusCode: StatusCodes.Status409Conflict),
                ERoomError.InvalidBlockPeriod => controller.Problem(
                    title: localizer["RoomBlockPeriodInvalid"].Value,
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