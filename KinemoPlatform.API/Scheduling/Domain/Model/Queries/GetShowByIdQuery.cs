namespace KinemoPlatform.API.Scheduling.Domain.Model.Queries;

/// <summary>
///     Query to retrieve a specific show by its unique identifier.
/// </summary>
/// <param name="ShowId">The unique identifier of the show.</param>
public record GetShowByIdQuery(int ShowId);