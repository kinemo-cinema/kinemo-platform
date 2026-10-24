using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Subscriptions.Domain.Repositories;

/// <summary>
///     Represents the subscription plan repository in the Kinemo Platform.
/// </summary>
public interface ISubscriptionPlanRepository : IBaseRepository<SubscriptionPlan>
{
    /// <summary>
    ///     Checks if a plan with the given name already exists.
    /// </summary>
    Task<bool> ExistsByNameAsync(string name);
}