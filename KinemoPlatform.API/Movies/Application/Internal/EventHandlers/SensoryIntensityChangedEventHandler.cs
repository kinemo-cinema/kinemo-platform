using KinemoPlatform.API.Movies.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Movies.Application.Internal.EventHandlers;

/// <summary>
///     Handles the <see cref="SensoryIntensityChangedEvent" /> domain event.
/// </summary>
/// <remarks>
///     In future iterations, BC08 (Operational Analytics) will consume this
///     event to track configuration changes over time.
/// </remarks>
public class SensoryIntensityChangedEventHandler : IEventHandler<SensoryIntensityChangedEvent>
{
    public Task Handle(SensoryIntensityChangedEvent domainEvent, CancellationToken cancellationToken)
    {
        return On(domainEvent);
    }

    private static Task On(SensoryIntensityChangedEvent domainEvent)
    {
        Console.WriteLine(
            "Changed intensity on Movie {0}: {1} -> {2}",
            domainEvent.MovieId,
            domainEvent.PreviousIntensity,
            domainEvent.NewIntensity);
        return Task.CompletedTask;
    }
}