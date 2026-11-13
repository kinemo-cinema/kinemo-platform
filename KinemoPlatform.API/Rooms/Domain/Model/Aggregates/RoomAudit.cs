using KinemoPlatform.API.Shared.Domain.Model;

namespace KinemoPlatform.API.Rooms.Domain.Model.Aggregates;

public partial class Room : IAuditableEntity
{ 
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}