using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Movies.Domain.Model.Entities;
using KinemoPlatform.API.Movies.Domain.Model.Queries;

namespace KinemoPlatform.API.Movies.Application.Services;

/// <summary>
///     Represents the movie query service in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Handles all read-only operations for the Movies bounded context.
/// </remarks>
public interface IMovieQueryService
{
    /// <summary>
    ///     Handle the get movie by id query.
    /// </summary>
    /// <param name="query">
    ///     The <see cref="GetMovieByIdQuery" /> to handle.
    /// </param>
    /// <returns>
    ///     The matching <see cref="Movie" />, or null if not found.
    /// </returns>
    Task<Movie?> Handle(GetMovieByIdQuery query);

    /// <summary>
    ///     Handle the get all movies query (US03).
    /// </summary>
    /// <param name="query">
    ///     The <see cref="GetAllMoviesQuery" /> to handle.
    /// </param>
    /// <returns>
    ///     A collection of all movies in the catalog.
    /// </returns>
    Task<IEnumerable<Movie>> Handle(GetAllMoviesQuery query);

    /// <summary>
    ///     Handle the search movies query (US05).
    /// </summary>
    /// <param name="query">
    ///     The <see cref="SearchMoviesQuery" /> to handle.
    /// </param>
    /// <returns>
    ///     A collection of movies matching the optional filters.
    /// </returns>
    Task<IEnumerable<Movie>> Handle(SearchMoviesQuery query);

    /// <summary>
    ///     Handle the get sensory content by movie id query.
    /// </summary>
    /// <param name="query">
    ///     The <see cref="GetSensoryContentByMovieIdQuery" /> to handle.
    /// </param>
    /// <returns>
    ///     The <see cref="SensoryContent" /> associated with the movie,
    ///     or null if the movie was not found.
    /// </returns>
    Task<SensoryContent?> Handle(GetSensoryContentByMovieIdQuery query);
}