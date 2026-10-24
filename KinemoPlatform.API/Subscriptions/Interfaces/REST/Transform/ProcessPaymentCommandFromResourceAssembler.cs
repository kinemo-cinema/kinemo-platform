using KinemoPlatform.API.Subscriptions.Domain.Model.Commands;
using KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Transform;
public static class ProcessPaymentCommandFromResourceAssembler
{
    public static ProcessPaymentCommand ToCommandFromResource(ProcessPaymentResource r, int subscriptionId) => 
        new(subscriptionId, r.Amount, r.PaymentToken);
}