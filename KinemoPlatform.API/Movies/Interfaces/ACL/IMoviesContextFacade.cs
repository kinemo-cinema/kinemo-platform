namespace KinemoPlatform.API.Movies.Interfaces.ACL;

public record MovieInfo(int MovieId, bool IsActive);

/// <summary>Facade that exposes Movies data to other bounded contexts.</summary>
public interface IMoviesContextFacade
{
    /// <summary>Basic info of a movie, or null when it does not exist.</summary>
    Task<MovieInfo?> FetchMovieInfo(int movieId, CancellationToken cancellationToken = default);
}