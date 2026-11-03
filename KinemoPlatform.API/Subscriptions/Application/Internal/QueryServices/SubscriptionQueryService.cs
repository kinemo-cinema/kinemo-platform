using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Model.Queries;
using KinemoPlatform.API.Subscriptions.Domain.Repositories;
using KinemoPlatform.API.Subscriptions.Application.Services;

namespace KinemoPlatform.API.Subscriptions.Application.Internal.QueryServices;

public class SubscriptionQueryService(
    ISubscriptionRepository subscriptionRepository) : ISubscriptionQueryService
{
    public async Task<Subscription?> Handle(
        GetSubscriptionByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await subscriptionRepository.FindByIdWithDetailsAsync(
            query.SubscriptionId, cancellationToken);
    }

    public async Task<Subscription?> Handle(
        GetSubscriptionByCinemaIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await subscriptionRepository.FindByCinemaIdAsync(
            query.CinemaId, cancellationToken);
    }
}