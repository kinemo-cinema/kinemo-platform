using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Model.Queries;
using KinemoPlatform.API.Subscriptions.Domain.Repositories;
using KinemoPlatform.API.Subscriptions.Domain.Services;

namespace KinemoPlatform.API.Subscriptions.Application.Internal.QueryServices;

public class SubscriptionQueryService(ISubscriptionRepository subscriptionRepository) : ISubscriptionQueryService
{
    public async Task<Subscription?> Handle(GetSubscriptionByIdQuery query)
    {
        return await subscriptionRepository.FindByIdWithDetailsAsync(query.SubscriptionId);
    }

    public async Task<Subscription?> Handle(GetSubscriptionByCinemaIdQuery query)
    {
        return await subscriptionRepository.FindByCinemaIdAsync(query.CinemaId);
    }
}