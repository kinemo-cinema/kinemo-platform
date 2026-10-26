using KinemoPlatform.API.Subscriptions.Domain.Model.Commands;
using KinemoPlatform.API.Subscriptions.Domain.Model.Entities;
using KinemoPlatform.API.Subscriptions.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;

/// <summary>
///     Subscription aggregate root.
/// </summary>
/// <remarks>
///     Manages the active contract for a cinema, its payment history, and plan changes.
/// </remarks>
public partial class Subscription
{
    public Subscription()
    {
        Status = ESubscriptionStatus.Active;
        Transactions = new List<PaymentTransaction>();
        Upgrades = new List<SubscriptionUpgrade>();
    }

    public Subscription(CreateSubscriptionCommand command) : this()
    {
        CinemaId = command.CinemaId;
        PlanId = command.PlanId;
        StartDate = DateTimeOffset.UtcNow;
        EndDate = DateTimeOffset.UtcNow.AddMonths(1); // Default 1 month billing cycle
    }

    public int Id { get; }
    public int CinemaId { get; private set; }
    public int PlanId { get; private set; }
    public ESubscriptionStatus Status { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }

    public ICollection<PaymentTransaction> Transactions { get; }
    public ICollection<SubscriptionUpgrade> Upgrades { get; }

    /// <summary>
    ///     Records a payment attempt.
    /// </summary>
    public PaymentTransaction ProcessPayment(decimal amount, bool isSuccessful)
    {
        var status = isSuccessful ? ETransactionStatus.Successful : ETransactionStatus.Failed;
        var transaction = new PaymentTransaction(amount, status);
        Transactions.Add(transaction);

        if (isSuccessful)
        {
            // Extend subscription by 1 month
            Status = ESubscriptionStatus.Active;
            EndDate = EndDate.AddMonths(1);
        }
        else
        {
            Status = ESubscriptionStatus.PastDue;
        }

        return transaction;
    }

    /// <summary>
    ///     Upgrades or downgrades the subscription to a new plan.
    /// </summary>
    public void ChangePlan(int newPlanId)
    {
        if (PlanId == newPlanId)
            throw new InvalidOperationException("The subscription is already on this plan.");

        Upgrades.Add(new SubscriptionUpgrade(PlanId, newPlanId));
        PlanId = newPlanId;
    }
}