using KinemoPlatform.API.Movies.Domain.Model.Entities;
using KinemoPlatform.API.Movies.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Transform;

/// <summary>
///     Assembler to convert a <see cref="ConfigurationHistoryEntry" /> entity
///     into a <see cref="ConfigurationHistoryResource" />.
/// </summary>
public static class ConfigurationHistoryResourceFromEntityAssembler
{
    /// <summary>
    ///     Convert a <see cref="ConfigurationHistoryEntry" /> entity into a
    ///     <see cref="ConfigurationHistoryResource" />.
    /// </summary>
    /// <param name="entity">
    ///     The <see cref="ConfigurationHistoryEntry" /> entity to convert.
    /// </param>
    /// <returns>
    ///     The resulting <see cref="ConfigurationHistoryResource" />.
    /// </returns>
    public static ConfigurationHistoryResource ToResourceFromEntity(ConfigurationHistoryEntry entity)
    {
        return new ConfigurationHistoryResource(
            entity.Id,
            entity.PreviousIntensity,
            entity.NewIntensity,
            entity.ChangedAt);
    }
}