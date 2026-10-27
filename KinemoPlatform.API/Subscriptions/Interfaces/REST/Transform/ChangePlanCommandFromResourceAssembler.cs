using KinemoPlatform.API.Subscriptions.Domain.Model.Commands;
using KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Transform;
public static class ChangePlanCommandFromResourceAssembler
{
    public static ChangePlanCommand ToCommandFromResource(ChangePlanResource r, int subscriptionId) => 
        new(subscriptionId, r.NewPlanId);
}