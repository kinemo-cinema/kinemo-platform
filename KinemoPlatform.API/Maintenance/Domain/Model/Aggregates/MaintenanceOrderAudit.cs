using KinemoPlatform.API.Shared.Domain.Model;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;

public partial class MaintenanceOrder : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}