using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Model.Queries;

namespace KinemoPlatform.API.Subscriptions.Application.Services;

public interface ISubscriptionQueryService
{
    Task<Subscription?> Handle(GetSubscriptionByIdQuery query);
    Task<Subscription?> Handle(GetSubscriptionByCinemaIdQuery query);
}