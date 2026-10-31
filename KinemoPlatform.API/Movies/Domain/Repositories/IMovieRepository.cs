using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Movies.Domain.Repositories;

/// <summary>
///     Represents the movie repository in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Extends the base repository with domain-specific query operations
///     needed by the Movies bounded context.
/// </remarks>
public interface IMovieRepository : IBaseRepository<Movie>
{
    /// <summary>
    ///     Verify whether a movie with the given title already exists.
    /// </summary>
    /// <param name="title">
    ///     The title to check.
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     True if a movie with the same title exists, false otherwise.
    /// </returns>
    Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find movies whose title contains the given text and/or whose
    ///     genre matches the given genre.
    /// </summary>
    /// <param name="title">
    ///     Optional partial title to match. Null means no title filter.
    /// </param>
    /// <param name="genre">
    ///     Optional genre to filter by. Null means no genre filter.
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A collection of matching movies.
    /// </returns>
    Task<IEnumerable<Movie>> FindByTitleOrGenreAsync(string? title, EMovieGenre? genre,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find a movie by its id, eagerly loading its sensory content
    ///     along with tracks and configuration history.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the movie.
    /// </param>
    /// <param name="cancellationToken">
    ///     Token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     The movie with sensory content loaded, or null if not found.
    /// </returns>
    Task<Movie?> FindByIdWithSensoryContentAsync(int id, CancellationToken cancellationToken = default);
}