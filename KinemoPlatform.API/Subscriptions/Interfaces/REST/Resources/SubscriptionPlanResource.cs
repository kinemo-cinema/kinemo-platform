using KinemoPlatform.API.Subscriptions.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;
public record SubscriptionPlanResource(int Id, EPlanType PlanType, string Name, decimal Price, int MaxScreens);