using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;
using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Movies.Domain.Model.Events;

/// <summary>
///     Domain event published when a new 4D movie is registered (US01).
/// </summary>
/// <param name="MovieId">
///     The unique identifier of the created movie.
/// </param>
/// <param name="Title">
///     The title of the created movie.
/// </param>
/// <param name="Genre">
///     The genre of the created movie.
/// </param>
public record MovieCreatedEvent(int MovieId, string Title, EMovieGenre Genre) : IEvent;