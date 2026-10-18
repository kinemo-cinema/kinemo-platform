using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Resources;

/// <summary>
///     Movie resource for REST API responses.
/// </summary>
/// <param name="Id">
///     The unique identifier of the movie.
/// </param>
/// <param name="Title">
///     The title of the movie.
/// </param>
/// <param name="DurationMinutes">
///     The duration of the movie in minutes.
/// </param>
/// <param name="Genre">
///     The <see cref="EMovieGenre" /> genre of the movie.
/// </param>
/// <param name="Status">
///     The <see cref="EMovieStatus" /> lifecycle status of the movie.
/// </param>
/// <param name="SensoryContent">
///     The sensory configuration associated with the movie.
/// </param>
public record MovieResource(
    int Id,
    string Title,
    int DurationMinutes,
    EMovieGenre Genre,
    EMovieStatus Status,
    SensoryContentResource SensoryContent);