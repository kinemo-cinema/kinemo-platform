using KinemoPlatform.API.Shared.Domain.Model;

namespace KinemoPlatform.API.Movies.Domain.Model.Entities;

public partial class SensoryContent : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}