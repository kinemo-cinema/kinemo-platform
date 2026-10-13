using KinemoPlatform.API.Movies.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Movies.Application.Internal.EventHandlers;

/// <summary>
///     Handles the <see cref="MovieDeactivatedEvent" /> domain event.
/// </summary>
/// <remarks>
///     In future iterations, BC02 (Scheduling) will consume this event
///     to prevent new shows from being assigned to the deactivated movie.
/// </remarks>
public class MovieDeactivatedEventHandler : IEventHandler<MovieDeactivatedEvent>
{
    public Task Handle(MovieDeactivatedEvent domainEvent, CancellationToken cancellationToken)
    {
        return On(domainEvent);
    }

    private static Task On(MovieDeactivatedEvent domainEvent)
    {
        Console.WriteLine("Deactivated Movie: {0}", domainEvent.MovieId);
        return Task.CompletedTask;
    }
}