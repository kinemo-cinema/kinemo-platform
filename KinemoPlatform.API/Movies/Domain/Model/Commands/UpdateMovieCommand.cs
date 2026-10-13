using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Domain.Model.Commands;

/// <summary>
///     Command to update the basic information of an existing movie.
/// </summary>
/// <param name="MovieId">
///     The unique identifier of the movie to update.
/// </param>
/// <param name="Title">
///     The new title of the movie.
/// </param>
/// <param name="DurationMinutes">
///     The new duration of the movie in minutes.
/// </param>
/// <param name="Genre">
///     The new <see cref="EMovieGenre" /> genre of the movie.
/// </param>
public record UpdateMovieCommand(int MovieId, string Title, int DurationMinutes, EMovieGenre Genre);