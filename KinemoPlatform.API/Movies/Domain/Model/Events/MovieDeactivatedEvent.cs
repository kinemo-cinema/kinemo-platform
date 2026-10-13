using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Movies.Domain.Model.Events;

/// <summary>
///     Domain event published when a movie is deactivated (US04).
/// </summary>
/// <remarks>
///     Consumed by BC02 (Scheduling) to prevent new shows from being
///     assigned to this movie.
/// </remarks>
/// <param name="MovieId">
///     The unique identifier of the deactivated movie.
/// </param>
public record MovieDeactivatedEvent(int MovieId) : IEvent;