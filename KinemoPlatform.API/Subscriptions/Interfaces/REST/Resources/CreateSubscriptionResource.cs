using System.ComponentModel.DataAnnotations;

namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;

public record CreateSubscriptionResource(
    [property: Range(1, int.MaxValue)] int CinemaId,
    [property: Range(1, int.MaxValue)] int PlanId
);