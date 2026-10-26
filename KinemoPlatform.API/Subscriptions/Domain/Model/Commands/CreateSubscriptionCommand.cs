namespace KinemoPlatform.API.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to create a new subscription for a cinema.
/// </summary>
public record CreateSubscriptionCommand(int CinemaId, int PlanId);