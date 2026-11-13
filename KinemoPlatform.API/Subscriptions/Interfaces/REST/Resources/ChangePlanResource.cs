using System.ComponentModel.DataAnnotations;

namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;

public record ChangePlanResource(
    [Range(1, int.MaxValue)] int NewPlanId
);