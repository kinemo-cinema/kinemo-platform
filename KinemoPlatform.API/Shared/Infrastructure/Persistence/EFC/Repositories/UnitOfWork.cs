using Cortex.Mediator;
using KinemoPlatform.API.Shared.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork(AppDbContext context, IMediator mediator) : IUnitOfWork
{
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
        await DispatchDomainEventsAsync(cancellationToken);
    }

    // Runs after the commit, so database-generated ids already exist and a failed save publishes nothing.
    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var aggregates = context.ChangeTracker.Entries<IAggregateRoot>()
            .Select(entry => entry.Entity)
            .Where(aggregate => aggregate.DomainEvents.Count > 0)
            .ToList();

        var events = aggregates.SelectMany(aggregate => aggregate.DomainEvents).ToList();
        foreach (var aggregate in aggregates) aggregate.ClearDomainEvents();   // clear first: handlers may save again

        foreach (var domainEvent in events)
            await mediator.PublishAsync(domainEvent, cancellationToken);
    }
}