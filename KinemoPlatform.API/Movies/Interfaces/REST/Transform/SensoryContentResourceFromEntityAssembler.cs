using KinemoPlatform.API.Movies.Domain.Model.Entities;
using KinemoPlatform.API.Movies.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Transform;

/// <summary>
///     Assembler to convert a <see cref="SensoryContent" /> entity into a
///     <see cref="SensoryContentResource" />.
/// </summary>
/// <remarks>
///     This assembler delegates to the track and history assemblers to
///     build the nested collections, keeping the aggregation logic in a
///     single place.
/// </remarks>
public static class SensoryContentResourceFromEntityAssembler
{
    /// <summary>
    ///     Convert a <see cref="SensoryContent" /> entity into a
    ///     <see cref="SensoryContentResource" />.
    /// </summary>
    /// <param name="entity">
    ///     The <see cref="SensoryContent" /> entity to convert.
    /// </param>
    /// <returns>
    ///     The resulting <see cref="SensoryContentResource" />.
    /// </returns>
    public static SensoryContentResource ToResourceFromEntity(SensoryContent entity)
    {
        return new SensoryContentResource(
            entity.Id,
            entity.IntensityLevel,
            entity.Status,
            entity.Tracks.Select(SensoryTrackResourceFromEntityAssembler.ToResourceFromEntity),
            entity.History.Select(ConfigurationHistoryResourceFromEntityAssembler.ToResourceFromEntity));
    }
}