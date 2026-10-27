using KinemoPlatform.API.Movies.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Movies.Application.Internal.EventHandlers;

/// <summary>
///     Handles the <see cref="MovieCreatedEvent" /> domain event.
/// </summary>
/// <remarks>
///     For now this handler only logs the event. In future iterations,
///     it can publish to BC08 (Operational Analytics) for tracking.
/// </remarks>
public class MovieCreatedEventHandler : IEventHandler<MovieCreatedEvent>
{
    public Task Handle(MovieCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        return On(domainEvent);
    }

    private static Task On(MovieCreatedEvent domainEvent)
    {
        Console.WriteLine(
            "Created Movie: {0} (Id {1}, Genre {2})",
            domainEvent.Title,
            domainEvent.MovieId,
            domainEvent.Genre);
        return Task.CompletedTask;
    }
}