using KinemoPlatform.API.Shared.Domain.Model;

namespace KinemoPlatform.API.Subscriptions.Domain.Model.Aggregates;

public partial class SubscriptionPlan : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}