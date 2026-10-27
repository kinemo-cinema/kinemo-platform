using KinemoPlatform.API.Scheduling.Domain.Model.Events;
using KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

namespace KinemoPlatform.API.Scheduling.Application.Internal.EventHandlers;

public class ShowCancelledEventHandler : IEventHandler<ShowCancelledEvent>
{
    public Task Handle(ShowCancelledEvent domainEvent, CancellationToken cancellationToken)
    {
        return On(domainEvent);
    }

    private static Task On(ShowCancelledEvent domainEvent)
    {
        Console.WriteLine("Cancelled Show: {0}", domainEvent.ShowId);
        return Task.CompletedTask;
    }
}