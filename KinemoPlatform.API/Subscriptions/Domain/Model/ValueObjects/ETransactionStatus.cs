namespace KinemoPlatform.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
///     Represents the outcome of a payment transaction.
/// </summary>
public enum ETransactionStatus
{
    Pending,
    Successful,
    Failed
}