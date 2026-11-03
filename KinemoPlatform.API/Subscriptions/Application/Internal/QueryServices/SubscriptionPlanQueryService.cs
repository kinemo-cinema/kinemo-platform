using KinemoPlatform.API.Subscriptions.Application.Services;
using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Model.Queries;
using KinemoPlatform.API.Subscriptions.Domain.Repositories;

namespace KinemoPlatform.API.Subscriptions.Application.Internal.QueryServices;

public class SubscriptionPlanQueryService(ISubscriptionPlanRepository planRepository) : ISubscriptionPlanQueryService
{
    public async Task<IEnumerable<SubscriptionPlan>> Handle(GetAllPlansQuery query)
    {
        return await planRepository.ListAsync();
    }
}