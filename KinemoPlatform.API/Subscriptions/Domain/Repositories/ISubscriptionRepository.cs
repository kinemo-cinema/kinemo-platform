using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Subscriptions.Domain.Repositories;

public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    Task<IEnumerable<Subscription>> ListWithDetailsAsync(CancellationToken cancellationToken = default);

    Task<Subscription?> FindByIdWithDetailsAsync(
        int subscriptionId,
        CancellationToken cancellationToken = default);

    Task<Subscription?> FindByCinemaIdAsync(
        int cinemaId,
        CancellationToken cancellationToken = default);
}