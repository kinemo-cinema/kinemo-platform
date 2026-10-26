using KinemoPlatform.API.Subscriptions.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;
public record PaymentTransactionResource(int Id, decimal Amount, ETransactionStatus Status, DateTimeOffset ProcessedAt);