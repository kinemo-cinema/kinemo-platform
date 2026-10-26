using Cortex.Mediator;
using KinemoPlatform.API.Shared.Domain.Repositories;
using KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;
using KinemoPlatform.API.Subscriptions.Domain.Model.Commands;
using KinemoPlatform.API.Subscriptions.Domain.Model.Events;
using KinemoPlatform.API.Subscriptions.Domain.Repositories;
using KinemoPlatform.API.Subscriptions.Domain.Services;

namespace KinemoPlatform.API.Subscriptions.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    ISubscriptionPlanRepository planRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : ISubscriptionCommandService
{
    public async Task<Subscription?> Handle(CreateSubscriptionCommand command)
    {
        var plan = await planRepository.FindByIdAsync(command.PlanId);
        if (plan is null) throw new InvalidOperationException($"Plan {command.PlanId} not found.");

        var subscription = new Subscription(command);
        await subscriptionRepository.AddAsync(subscription);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new SubscriptionCreatedEvent(subscription.Id, subscription.CinemaId, subscription.PlanId));

        return subscription;
    }

    public async Task<Subscription?> Handle(ProcessPaymentCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdWithDetailsAsync(command.SubscriptionId);
        if (subscription is null) throw new InvalidOperationException($"Subscription {command.SubscriptionId} not found.");

        // SIMULATION: In a real app, you would call Stripe/PayPal here using the PaymentToken.
        // For this project, we simulate success if the token is not "fail".
        bool isSuccessful = command.PaymentToken.ToLower() != "fail";

        var transaction = subscription.ProcessPayment(command.Amount, isSuccessful);
        
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new PaymentProcessedEvent(subscription.Id, transaction.Amount, isSuccessful));

        return subscription;
    }

    public async Task<Subscription?> Handle(ChangePlanCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdWithDetailsAsync(command.SubscriptionId);
        if (subscription is null) throw new InvalidOperationException($"Subscription {command.SubscriptionId} not found.");

        var newPlan = await planRepository.FindByIdAsync(command.NewPlanId);
        if (newPlan is null) throw new InvalidOperationException($"Plan {command.NewPlanId} not found.");

        subscription.ChangePlan(command.NewPlanId);
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();

        return subscription;
    }
}