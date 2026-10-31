using KinemoPlatform.API.Movies.Application.Services;
using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Movies.Domain.Model.Entities;
using KinemoPlatform.API.Movies.Domain.Model.Queries;
using KinemoPlatform.API.Movies.Domain.Repositories;

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
    public async Task<Movie?> Handle(GetMovieByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await movieRepository.FindByIdAsync(query.MovieId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Movie>> Handle(GetAllMoviesQuery query, CancellationToken cancellationToken = default)
    {
        return await movieRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Movie>> Handle(SearchMoviesQuery query, CancellationToken cancellationToken = default)
    {
        return await movieRepository.FindByTitleOrGenreAsync(query.Title, query.Genre, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<SensoryContent?> Handle(GetSensoryContentByMovieIdQuery query, CancellationToken cancellationToken = default)
    {
        var movie = await movieRepository.FindByIdWithSensoryContentAsync(query.MovieId, cancellationToken);
        return movie?.SensoryContent;
    }
}