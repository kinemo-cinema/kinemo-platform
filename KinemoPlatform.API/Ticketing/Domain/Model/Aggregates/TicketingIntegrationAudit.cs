using KinemoPlatform.API.Shared.Domain.Model;

namespace KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;

public partial class TicketingIntegration : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}