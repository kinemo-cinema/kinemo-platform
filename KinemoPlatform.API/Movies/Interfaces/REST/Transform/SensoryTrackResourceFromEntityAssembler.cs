using KinemoPlatform.API.Movies.Domain.Model.Entities;
using KinemoPlatform.API.Movies.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Transform;

/// <summary>
///     Assembler to convert a <see cref="SensoryTrack" /> entity into a
///     <see cref="SensoryTrackResource" />.
/// </summary>
public static class SensoryTrackResourceFromEntityAssembler
{
    /// <summary>
    ///     Convert a <see cref="SensoryTrack" /> entity into a
    ///     <see cref="SensoryTrackResource" />.
    /// </summary>
    /// <param name="entity">
    ///     The <see cref="SensoryTrack" /> entity to convert.
    /// </param>
    /// <returns>
    ///     The resulting <see cref="SensoryTrackResource" />.
    /// </returns>
    public static SensoryTrackResource ToResourceFromEntity(SensoryTrack entity)
    {
        return new SensoryTrackResource(
            entity.Id,
            entity.EffectType,
            entity.FilePath,
            entity.DurationSeconds);
    }
}