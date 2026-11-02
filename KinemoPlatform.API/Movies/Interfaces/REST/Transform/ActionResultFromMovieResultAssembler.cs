using KinemoPlatform.API.Movies.Application.Errors;
using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Transform;

/// <summary>
///     Maps the <see cref="Result{TValue,TError}" /> of a movie command to an HTTP response.
/// </summary>
/// <remarks>
///     Failures are returned as RFC 7807 <c>application/problem+json</c> with a localized title,
///     so every error of the API shares the same shape.
/// </remarks>
public static class ActionResultFromMovieResultAssembler
{
    /// <summary>
    ///     Converts a command result into an <see cref="IActionResult" />.
    /// </summary>
    /// <param name="result">The result returned by the movie command service.</param>
    /// <param name="controller">The controller used to build the problem responses.</param>
    /// <param name="localizer">Localizer used for the problem titles.</param>
    /// <param name="onSuccess">Builds the success response (200 OK, 201 Created, ...) from the movie.</param>
    /// <returns>The HTTP response for the result.</returns>
    public static IActionResult ToActionResult(
        Result<Movie, EMovieError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        Func<Movie, IActionResult> onSuccess)
    {
        return result switch
        {
            Result<Movie, EMovieError>.Success success => onSuccess(success.Value),
            Result<Movie, EMovieError>.Failure failure => failure.Error switch
            {
                EMovieError.MovieNotFound => controller.Problem(
                    title: localizer["MovieNotFound"].Value,
                    statusCode: StatusCodes.Status404NotFound),
                EMovieError.DuplicateTitle => controller.Problem(
                    title: localizer["MovieTitleDuplicated"].Value,
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