using KinemoPlatform.API.Movies.Application.Errors;
using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Movies.Domain.Model.Commands;
using KinemoPlatform.API.Shared.Application.Patterns;

namespace KinemoPlatform.API.Movies.Application.Services;

/// <summary>
///     Represents the movie command service in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Handles all state-changing operations for the Movies bounded context.
/// </remarks>
public interface IMovieCommandService
{
    /// <summary>
    ///     Handle the create movie command (US01).
    /// </summary>
    /// <param name="command">
    ///     The <see cref="CreateMovieCommand" /> to handle.
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A success result with the affected <see cref="Movie" />, or a failure with an <see cref="EMovieError" />.
    /// </returns>
    Task<Result<Movie, EMovieError>> Handle(CreateMovieCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update movie command.
    /// </summary>
    /// <param name="command">
    ///     The <see cref="UpdateMovieCommand" /> to handle.
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A success result with the affected <see cref="Movie" />, or a failure with an <see cref="EMovieError" />.
    /// </returns>
    Task<Result<Movie, EMovieError>> Handle(UpdateMovieCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the deactivate movie command (US04).
    /// </summary>
    /// <param name="command">
    ///     The <see cref="DeactivateMovieCommand" /> to handle.
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A success result with the affected <see cref="Movie" />, or a failure with an <see cref="EMovieError" />.
    /// </returns>
    Task<Result<Movie, EMovieError>> Handle(DeactivateMovieCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the associate sensory file command (US02).
    /// </summary>
    /// <param name="command">
    ///     The <see cref="AssociateSensoryFileCommand" /> to handle.
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A success result with the affected <see cref="Movie" />, or a failure with an <see cref="EMovieError" />.
    /// </returns>
    Task<Result<Movie, EMovieError>> Handle(AssociateSensoryFileCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the configure effects profile command (US06).
    /// </summary>
    /// <param name="command">
    ///     The <see cref="ConfigureEffectsProfileCommand" /> to handle.
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A success result with the affected <see cref="Movie" />, or a failure with an <see cref="EMovieError" />.
    /// </returns>
    Task<Result<Movie, EMovieError>> Handle(ConfigureEffectsProfileCommand command, CancellationToken cancellationToken = default);
}