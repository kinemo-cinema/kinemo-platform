namespace KinemoPlatform.API.Movies.Domain.Model.ValueObjects;


/// <summary>
///     Represents the lifecycle status of a movie in the Kinemo Platform.
/// </summary>
/// <remarks>
///     A movie marked as <see cref="Inactive" /> cannot be scheduled for new shows,
///     but remains in the catalog for historical reference.
/// </remarks>
public enum EMovieStatus
{
    Active,
    Inactive
}