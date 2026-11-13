using System.ComponentModel.DataAnnotations;

namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;

public record CreateSubscriptionResource(
    [Range(1, int.MaxValue)] int CinemaId,
    [Range(1, int.MaxValue)] int PlanId
);