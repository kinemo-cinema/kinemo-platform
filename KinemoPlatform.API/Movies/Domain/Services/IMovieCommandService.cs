using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Movies.Domain.Model.Commands;

namespace KinemoPlatform.API.Movies.Domain.Services;

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
    /// <returns>
    ///     The created <see cref="Movie" />, or null if the operation failed.
    /// </returns>
    Task<Movie?> Handle(CreateMovieCommand command);

    /// <summary>
    ///     Handle the update movie command.
    /// </summary>
    /// <param name="command">
    ///     The <see cref="UpdateMovieCommand" /> to handle.
    /// </param>
    /// <returns>
    ///     The updated <see cref="Movie" />, or null if the movie was not found.
    /// </returns>
    Task<Movie?> Handle(UpdateMovieCommand command);

    /// <summary>
    ///     Handle the deactivate movie command (US04).
    /// </summary>
    /// <param name="command">
    ///     The <see cref="DeactivateMovieCommand" /> to handle.
    /// </param>
    /// <returns>
    ///     The deactivated <see cref="Movie" />, or null if not found.
    /// </returns>
    Task<Movie?> Handle(DeactivateMovieCommand command);

    /// <summary>
    ///     Handle the associate sensory file command (US02).
    /// </summary>
    /// <param name="command">
    ///     The <see cref="AssociateSensoryFileCommand" /> to handle.
    /// </param>
    /// <returns>
    ///     The updated <see cref="Movie" />, or null if the movie was not found.
    /// </returns>
    Task<Movie?> Handle(AssociateSensoryFileCommand command);

    /// <summary>
    ///     Handle the configure effects profile command (US06).
    /// </summary>
    /// <param name="command">
    ///     The <see cref="ConfigureEffectsProfileCommand" /> to handle.
    /// </param>
    /// <returns>
    ///     The updated <see cref="Movie" />, or null if the movie was not found.
    /// </returns>
    Task<Movie?> Handle(ConfigureEffectsProfileCommand command);
}