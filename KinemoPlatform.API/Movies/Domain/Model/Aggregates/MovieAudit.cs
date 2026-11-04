using KinemoPlatform.API.Shared.Domain.Model;

namespace KinemoPlatform.API.Movies.Domain.Model.Aggregates;

public partial class Movie : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}