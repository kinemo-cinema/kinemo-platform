namespace KinemoPlatform.API.Movies.Domain.Model.Commands;

/// <summary>
///     Command to deactivate a movie, preventing it from being scheduled
///     in new shows while preserving it in the catalog.
/// </summary>
/// <param name="MovieId">
///     The unique identifier of the movie to deactivate.
/// </param>
public record DeactivateMovieCommand(int MovieId);