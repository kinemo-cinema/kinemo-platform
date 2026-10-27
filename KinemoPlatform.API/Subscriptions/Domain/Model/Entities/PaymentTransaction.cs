using KinemoPlatform.API.Subscriptions.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Subscriptions.Domain.Model.Entities;

public class PaymentTransaction
{
    public PaymentTransaction()
    {
    }

    public PaymentTransaction(decimal amount, ETransactionStatus status) : this()
    {
        Amount = amount;
        Status = status;
        ProcessedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; }
    public int SubscriptionId { get; private set; }
    public decimal Amount { get; private set; }
    public ETransactionStatus Status { get; private set; }
    public DateTimeOffset ProcessedAt { get; private set; }
}