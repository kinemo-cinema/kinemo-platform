namespace KinemoPlatform.API.Movies.Domain.Model.ValueObjects;


/// <summary>
///     Represents the preparation status of a movie's sensory content in the Kinemo Platform.
/// </summary>
/// <remarks>
///     A movie can only be executed on a 4D room when its sensory content reaches
///     <see cref="Active" /> status.
/// </remarks>
public enum ESensoryContentStatus
{
    Draft,
    Ready,
    Active,
    Inactive
}