using KinemoPlatform.API.Shared.Domain.Model;


namespace KinemoPlatform.API.Seats.Domain.Model.Aggregates;

public partial class SeatAllocation : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}