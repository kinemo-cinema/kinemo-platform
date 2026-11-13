using KinemoPlatform.API.Execution.Application.Errors;
using KinemoPlatform.API.Execution.Domain.Model.Aggregates;
using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Transform;

/// <summary>
///     Maps the <see cref="Result{TValue,TError}" /> of an execution command to an HTTP response.
/// </summary>
/// <remarks>
///     Failures are returned as RFC 7807 <c>application/problem+json</c> with a localized title.
/// </remarks>
public static class ActionResultFromExecutionResultAssembler
{
    /// <summary>
    ///     Converts a command result into an <see cref="IActionResult" />.
    /// </summary>
    /// <param name="result">The result returned by the execution command service.</param>
    /// <param name="controller">The controller used to build the problem responses.</param>
    /// <param name="localizer">Localizer used for the problem titles.</param>
    /// <param name="onSuccess">Builds the success response (200 OK, 201 Created, ...) from the execution.</param>
    /// <returns>The HTTP response for the result.</returns>
    public static IActionResult ToActionResult(
        Result<ShowExecution, EExecutionError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        Func<ShowExecution, IActionResult> onSuccess)
    {
        return result switch
        {
            Result<ShowExecution, EExecutionError>.Success success => onSuccess(success.Value),
            Result<ShowExecution, EExecutionError>.Failure failure => failure.Error switch
            {
                EExecutionError.ExecutionNotFound => controller.Problem(
                    title: localizer["ExecutionNotFound"].Value,
                    statusCode: StatusCodes.Status404NotFound),
                EExecutionError.ActiveExecutionExists => controller.Problem(
                    title: localizer["ActiveExecutionAlreadyExists"].Value,
                    statusCode: StatusCodes.Status409Conflict),
                EExecutionError.InvalidExecutionState => controller.Problem(
                    title: localizer["InvalidExecutionState"].Value,
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