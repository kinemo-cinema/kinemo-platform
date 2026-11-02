using KinemoPlatform.API.Maintenance.Application.Errors;
using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;

/// <summary>
///     Maps the <see cref="Result{TValue,TError}" /> of an equipment command to an HTTP response.
/// </summary>
/// <remarks>
///     Failures are returned as RFC 7807 <c>application/problem+json</c> with a localized title.
/// </remarks>
public static class ActionResultFromEquipmentResultAssembler
{
    /// <summary>
    ///     Converts a command result into an <see cref="IActionResult" />.
    /// </summary>
    /// <param name="result">The result returned by the equipment command service.</param>
    /// <param name="controller">The controller used to build the problem responses.</param>
    /// <param name="localizer">Localizer used for the problem titles.</param>
    /// <param name="onSuccess">Builds the success response (200 OK, 201 Created, ...) from the equipment.</param>
    /// <returns>The HTTP response for the result.</returns>
    public static IActionResult ToActionResult(
        Result<Equipment, EEquipmentError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        Func<Equipment, IActionResult> onSuccess)
    {
        return result switch
        {
            Result<Equipment, EEquipmentError>.Success success => onSuccess(success.Value),
            Result<Equipment, EEquipmentError>.Failure failure => failure.Error switch
            {
                EEquipmentError.EquipmentNotFound => controller.Problem(
                    title: localizer["EquipmentNotFound"].Value,
                    statusCode: StatusCodes.Status404NotFound),
                EEquipmentError.DuplicateEquipment => controller.Problem(
                    title: localizer["EquipmentNameDuplicated"].Value,
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
