namespace KinemoPlatform.API.Movies.Domain.Model.Queries;

/// <summary>
///     Query to retrieve a single movie by its unique identifier.
/// </summary>
/// <param name="MovieId">
///     The unique identifier of the movie to retrieve.
/// </param>
public record GetMovieByIdQuery(int MovieId);