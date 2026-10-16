using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Movies.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Transform;

/// <summary>
///     Assembler to convert a <see cref="Movie" /> aggregate into a
///     <see cref="MovieResource" />.
/// </summary>
public static class MovieResourceFromEntityAssembler
{
    /// <summary>
    ///     Convert a <see cref="Movie" /> aggregate into a
    ///     <see cref="MovieResource" />.
    /// </summary>
    /// <param name="entity">
    ///     The <see cref="Movie" /> aggregate to convert.
    /// </param>
    /// <returns>
    ///     The resulting <see cref="MovieResource" />.
    /// </returns>
    public static MovieResource ToResourceFromEntity(Movie entity)
    {
        return new MovieResource(
            entity.Id,
            entity.Title,
            entity.DurationMinutes,
            entity.Genre,
            entity.Status,
            SensoryContentResourceFromEntityAssembler.ToResourceFromEntity(entity.SensoryContent));
    }
}