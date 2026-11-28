using KinemoPlatform.API.Movies.Application.Services;
using KinemoPlatform.API.Movies.Domain.Model.Queries;
using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;
using KinemoPlatform.API.Movies.Interfaces.ACL;

namespace KinemoPlatform.API.Movies.Application.ACL;

public class MoviesContextFacade(IMovieQueryService movieQueryService) : IMoviesContextFacade
{
    public async Task<MovieInfo?> FetchMovieInfo(int movieId, CancellationToken cancellationToken = default)
    {
        var movie = await movieQueryService.Handle(new GetMovieByIdQuery(movieId), cancellationToken);
        return movie is null ? null : new MovieInfo(movie.Id, movie.Status == EMovieStatus.Active);
    }
}