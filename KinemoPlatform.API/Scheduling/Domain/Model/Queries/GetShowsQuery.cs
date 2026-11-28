using KinemoPlatform.API.Scheduling.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Scheduling.Domain.Model.Queries;

/// <summary>Filtered list of shows. Every filter is optional and filters combine with AND.</summary>
public record GetShowsQuery(
    DateTimeOffset? Date = null,
    int? RoomId = null,
    int? MovieId = null,
    EShowStatus? Status = null);