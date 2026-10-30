using KinemoPlatform.API.Movies.Application.Services;
using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Movies.Domain.Model.Entities;
using KinemoPlatform.API.Movies.Domain.Model.Queries;
using KinemoPlatform.API.Movies.Domain.Repositories;
using KinemoPlatform.API.Movies.Domain.Services;

namespace KinemoPlatform.API.Movies.Application.Internal.QueryServices;

/// <summary>
///     Represents the movie query service in the Kinemo Platform.
/// </summary>
/// <param name="movieRepository">
///     The <see cref="IMovieRepository" /> to use.
/// </param>
public class MovieQueryService(IMovieRepository movieRepository) : IMovieQueryService
{
    /// <inheritdoc />
    public async Task<Movie?> Handle(GetMovieByIdQuery query)
    {
        return await movieRepository.FindByIdAsync(query.MovieId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Movie>> Handle(GetAllMoviesQuery query)
    {
        return await movieRepository.ListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Movie>> Handle(SearchMoviesQuery query)
    {
        return await movieRepository.FindByTitleOrGenreAsync(query.Title, query.Genre);
    }

    /// <inheritdoc />
    public async Task<SensoryContent?> Handle(GetSensoryContentByMovieIdQuery query)
    {
        var movie = await movieRepository.FindByIdWithSensoryContentAsync(query.MovieId);
        return movie?.SensoryContent;
    }
}