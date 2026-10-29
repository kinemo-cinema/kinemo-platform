using KinemoPlatform.API.Shared.Domain.Model;

namespace KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;

public partial class Show : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}