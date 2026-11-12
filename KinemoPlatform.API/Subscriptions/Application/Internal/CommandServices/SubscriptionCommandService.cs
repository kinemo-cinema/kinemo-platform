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

    public Task<Subscription?> Handle(ProcessPaymentCommand command) =>
        Task.FromException<Subscription?>(new NotSupportedException("No real payment provider is configured. No payment was processed."));

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