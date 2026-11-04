using KinemoPlatform.API.Shared.Domain.Model;

namespace KinemoPlatform.API.Analytics.Domain.Model.Aggregates;

public partial class OperationalReport : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}