using KinemoPlatform.API.Shared.Domain.Model;

namespace KinemoPlatform.API.Analytics.Domain.Model.Aggregates;

public partial class Dashboard : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
