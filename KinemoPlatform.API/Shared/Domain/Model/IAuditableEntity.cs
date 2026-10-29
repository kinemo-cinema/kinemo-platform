namespace KinemoPlatform.API.Shared.Domain.Model;

/// <summary>
///     Contract for entities that keep audit timestamps.
/// </summary>
/// <remarks>
///     The timestamps are maintained automatically by
///     <c>AuditableEntityInterceptor</c> when changes are saved.
/// </remarks>
public interface IAuditableEntity
{
    /// <summary>
    ///     Moment at which the entity was first persisted.
    /// </summary>
    DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    ///     Moment at which the entity was last persisted.
    /// </summary>
    DateTimeOffset? UpdatedAt { get; set; }
}