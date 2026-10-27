using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Domain.Model.Queries;

/// <summary>
///     Query to search movies by title and/or genre (US05).
/// </summary>
/// <remarks>
///     Both criteria are optional. When both are null, the query behaves
///     like <see cref="GetAllMoviesQuery" />.
/// </remarks>
/// <param name="Title">
///     Optional partial title to match.
/// </param>
/// <param name="Genre">
///     Optional <see cref="EMovieGenre" /> to filter by.
/// </param>
public record SearchMoviesQuery(string? Title, EMovieGenre? Genre);