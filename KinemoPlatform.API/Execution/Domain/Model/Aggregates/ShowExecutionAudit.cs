using KinemoPlatform.API.Shared.Domain.Model;

namespace KinemoPlatform.API.Execution.Domain.Model.Aggregates;

public partial class ShowExecution : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}