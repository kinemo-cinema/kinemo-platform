using KinemoPlatform.API.Maintenance.Application.Errors;
using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;

/// <summary>
///     Maps the <see cref="Result{TValue,TError}" /> of a maintenance order command to an HTTP response.
/// </summary>
/// <remarks>
///     Failures are returned as RFC 7807 <c>application/problem+json</c> with a localized title.
///     When scheduling, a missing equipment is a 400: the route is valid but the request body refers to something
///     that does not exist. A missing order (the id is in the route) is a 404.
/// </remarks>
public static class ActionResultFromMaintenanceOrderResultAssembler
{
    /// <summary>
    ///     Converts a command result into an <see cref="IActionResult" />.
    /// </summary>
    /// <param name="result">The result returned by the maintenance order command service.</param>
    /// <param name="controller">The controller used to build the problem responses.</param>
    /// <param name="localizer">Localizer used for the problem titles.</param>
    /// <param name="onSuccess">Builds the success response (200 OK, 201 Created, ...) from the order.</param>
    /// <returns>The HTTP response for the result.</returns>
    public static IActionResult ToActionResult(
        Result<MaintenanceOrder, EMaintenanceOrderError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        Func<MaintenanceOrder, IActionResult> onSuccess)
    {
        return result switch
        {
            Result<MaintenanceOrder, EMaintenanceOrderError>.Success success => onSuccess(success.Value),
            Result<MaintenanceOrder, EMaintenanceOrderError>.Failure failure => failure.Error switch
            {
                EMaintenanceOrderError.EquipmentNotFound => controller.Problem(
                    title: localizer["EquipmentNotFound"].Value,
                    statusCode: StatusCodes.Status400BadRequest),
                EMaintenanceOrderError.OrderNotFound => controller.Problem(
                    title: localizer["MaintenanceOrderNotFound"].Value,
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
