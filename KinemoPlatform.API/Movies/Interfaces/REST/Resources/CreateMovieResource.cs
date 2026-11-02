using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Resources;

/// <summary>
///     Resource to create a new 4D movie in the Kinemo Platform (US01).
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
public record CreateMovieResource(
    [Required(ErrorMessage = "RequiredMovieTitle")]
    [StringLength(200, ErrorMessage = "InvalidMovieTitleLength")]
    string Title,
    [Range(1, 600, ErrorMessage = "InvalidMovieDuration")]
    int DurationMinutes,
    [EnumDataType(typeof(EMovieGenre), ErrorMessage = "InvalidMovieGenre")]
    EMovieGenre Genre);