namespace KinemoPlatform.API.Movies.Domain.Model.Queries;

/// <summary>
///     Query to retrieve the sensory configuration associated with a movie.
/// </summary>
/// <param name="MovieId">
///     The unique identifier of the movie whose sensory content is requested.
/// </param>
public record GetSensoryContentByMovieIdQuery(int MovieId);