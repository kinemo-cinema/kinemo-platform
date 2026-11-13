using Cortex.Mediator;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;
using KinemoPlatform.API.Subscriptions.Application.Errors;
using KinemoPlatform.API.Subscriptions.Application.Services;
using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Model.Commands;
using KinemoPlatform.API.Subscriptions.Domain.Model.Events;
using KinemoPlatform.API.Subscriptions.Domain.Repositories;

namespace KinemoPlatform.API.Subscriptions.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    ISubscriptionPlanRepository planRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : ISubscriptionCommandService
{
    public async Task<Result<Subscription, ESubscriptionError>> Handle(
        CreateSubscriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        var plan = await planRepository.FindByIdAsync(
            command.PlanId, cancellationToken);

        if (plan is null)
            return new Result<Subscription, ESubscriptionError>.Failure(
                ESubscriptionError.PlanNotFound);

        var subscription = new Subscription(command);

        await subscriptionRepository.AddAsync(subscription, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new SubscriptionCreatedEvent(
                subscription.Id,
                subscription.CinemaId,
                subscription.PlanId),
            cancellationToken);

        return new Result<Subscription, ESubscriptionError>.Success(subscription);
    }

    public async Task<Result<Subscription, ESubscriptionError>> Handle(
        ProcessPaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        var subscription = await subscriptionRepository.FindByIdWithDetailsAsync(
            command.SubscriptionId, cancellationToken);

        if (subscription is null)
            return new Result<Subscription, ESubscriptionError>.Failure(
                ESubscriptionError.SubscriptionNotFound);

        if (command.Amount <= 0 || string.IsNullOrWhiteSpace(command.PaymentToken))
            return new Result<Subscription, ESubscriptionError>.Failure(
                ESubscriptionError.InvalidPayment);

        return new Result<Subscription, ESubscriptionError>.Failure(
            ESubscriptionError.PaymentProviderUnavailable);
    }

    public async Task<Result<Subscription, ESubscriptionError>> Handle(
        ChangePlanCommand command,
        CancellationToken cancellationToken = default)
    {
        var subscription = await subscriptionRepository.FindByIdWithDetailsAsync(
            command.SubscriptionId, cancellationToken);

        if (subscription is null)
            return new Result<Subscription, ESubscriptionError>.Failure(
                ESubscriptionError.SubscriptionNotFound);

        var newPlan = await planRepository.FindByIdAsync(
            command.NewPlanId, cancellationToken);

        if (newPlan is null)
            return new Result<Subscription, ESubscriptionError>.Failure(
                ESubscriptionError.PlanNotFound);

        if (subscription.PlanId == command.NewPlanId)
            return new Result<Subscription, ESubscriptionError>.Failure(
                ESubscriptionError.InvalidPlanChange);

        subscription.ChangePlan(command.NewPlanId);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync(cancellationToken);

        return new Result<Subscription, ESubscriptionError>.Success(subscription);
    }
}
