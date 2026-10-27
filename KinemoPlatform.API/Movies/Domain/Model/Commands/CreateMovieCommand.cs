using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Domain.Model.Commands;

/// <summary>
///     Command to register a new 4D movie in the Kinemo Platform.
/// </summary>
/// <param name="Title">
///     The title of the movie.
/// </param>
/// <param name="DurationMinutes">
///     The total duration of the movie in minutes.
/// </param>
/// <param name="Genre">
///     The <see cref="EMovieGenre" /> genre of the movie.
/// </param>
public record CreateMovieCommand(string Title, int DurationMinutes, EMovieGenre Genre);