using KinemoPlatform.API.Movies.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Movies.Application.Internal.EventHandlers;

/// <summary>
///     Handles the <see cref="SensoryFileAssociatedEvent" /> domain event.
/// </summary>
/// <remarks>
///     In future iterations, BC07 (Resource Testing &amp; Maintenance) will
///     consume this event to trigger a channel test on the new effect.
/// </remarks>
public class SensoryFileAssociatedEventHandler : IEventHandler<SensoryFileAssociatedEvent>
{
    public Task Handle(SensoryFileAssociatedEvent domainEvent, CancellationToken cancellationToken)
    {
        return On(domainEvent);
    }

    private static Task On(SensoryFileAssociatedEvent domainEvent)
    {
        Console.WriteLine(
            "Associated sensory file to Movie {0}: {1} -> {2}",
            domainEvent.MovieId,
            domainEvent.EffectType,
            domainEvent.FilePath);
        return Task.CompletedTask;
    }
}