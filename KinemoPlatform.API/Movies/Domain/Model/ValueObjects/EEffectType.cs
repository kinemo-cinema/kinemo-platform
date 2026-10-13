namespace KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

/// <summary>
///     Represents the type of physical effect associated with a sensory track.
/// </summary>
/// <remarks>
///     Each effect type corresponds to a physical channel available on the 4D seats
///     and the room's environmental system.
/// </remarks>
public enum EEffectType
{
    Motion,
    Wind,
    Water,
    Vibration
}