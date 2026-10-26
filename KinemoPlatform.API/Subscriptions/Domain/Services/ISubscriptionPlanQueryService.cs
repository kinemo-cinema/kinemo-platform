using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Model.Queries;

namespace KinemoPlatform.API.Subscriptions.Domain.Services;

public interface ISubscriptionPlanQueryService
{
    /// <summary>
    ///     Handles retrieving all available plans (for the landing page and pricing tables).
    /// </summary>
    Task<IEnumerable<SubscriptionPlan>> Handle(GetAllPlansQuery query);
}