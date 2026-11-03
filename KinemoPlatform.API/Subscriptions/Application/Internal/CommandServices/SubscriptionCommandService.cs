
using Cortex.Mediator;
using KinemoPlatform.API.Shared.Domain.Repositories;
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
    public async Task<Subscription?> Handle(
        CreateSubscriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        var plan = await planRepository.FindByIdAsync(command.PlanId, cancellationToken);
        if (plan is null)
            throw new InvalidOperationException($"Plan {command.PlanId} not found.");

        var subscription = new Subscription(command);

        await subscriptionRepository.AddAsync(subscription, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new SubscriptionCreatedEvent(
                subscription.Id,
                subscription.CinemaId,
                subscription.PlanId),
            cancellationToken);

        return subscription;
    }

    public async Task<Subscription?> Handle(
        ProcessPaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        var subscription = await subscriptionRepository.FindByIdWithDetailsAsync(
            command.SubscriptionId, cancellationToken);

        if (subscription is null)
            throw new InvalidOperationException(
                $"Subscription {command.SubscriptionId} not found.");

        bool isSuccessful = command.PaymentToken.ToLower() != "fail";

        var transaction = subscription.ProcessPayment(command.Amount, isSuccessful);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new PaymentProcessedEvent(
                subscription.Id,
                transaction.Amount,
                isSuccessful),
            cancellationToken);

        return subscription;
    }

    public async Task<Subscription?> Handle(
        ChangePlanCommand command,
        CancellationToken cancellationToken = default)
    {
        var subscription = await subscriptionRepository.FindByIdWithDetailsAsync(
            command.SubscriptionId, cancellationToken);

        if (subscription is null)
            throw new InvalidOperationException(
                $"Subscription {command.SubscriptionId} not found.");

        var newPlan = await planRepository.FindByIdAsync(command.NewPlanId, cancellationToken);
        if (newPlan is null)
            throw new InvalidOperationException(
                $"Plan {command.NewPlanId} not found.");

        subscription.ChangePlan(command.NewPlanId);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync(cancellationToken);

        return subscription;
    }
}
