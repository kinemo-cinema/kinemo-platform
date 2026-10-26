using KinemoPlatform.API.Subscriptions.Domain.Model.Entities;
using KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Transform;
public static class PaymentTransactionResourceFromEntityAssembler
{
    public static PaymentTransactionResource ToResourceFromEntity(PaymentTransaction e) => 
        new(e.Id, e.Amount, e.Status, e.ProcessedAt);
}