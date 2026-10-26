using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Subscriptions.Domain.Repositories;

/// <summary>
///     Represents the subscription repository in the Kinemo Platform.
/// </summary>
public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    /// <summary>
    ///     Retrieves a subscription by ID, eagerly loading its payments and upgrades.
    /// </summary>
    Task<Subscription?> FindByIdWithDetailsAsync(int subscriptionId);

    /// <summary>
    ///     Retrieves the active (or most recent) subscription for a specific cinema.
    /// </summary>
    Task<Subscription?> FindByCinemaIdAsync(int cinemaId);
}