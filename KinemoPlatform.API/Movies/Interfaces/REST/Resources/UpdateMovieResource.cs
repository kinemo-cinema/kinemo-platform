using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Resources;

/// <summary>
///     Resource to update the basic information of an existing movie.
/// </summary>
/// <param name="Title">
///     The new title of the movie.
/// </param>
/// <param name="DurationMinutes">
///     The new duration of the movie in minutes.
/// </param>
/// <param name="Genre">
///     The new <see cref="EMovieGenre" /> genre of the movie.
/// </param>
public record UpdateMovieResource(
    string Title,
    int DurationMinutes,
    EMovieGenre Genre);