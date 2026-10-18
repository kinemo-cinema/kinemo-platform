using KinemoPlatform.API.Movies.Domain.Model.Commands;
using KinemoPlatform.API.Movies.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Transform;

/// <summary>
///     Assembler to create a <see cref="ConfigureEffectsProfileCommand" />
///     from a <see cref="ConfigureEffectsProfileResource" /> and a movie id
///     from the route.
/// </summary>
public static class ConfigureEffectsProfileCommandFromResourceAssembler
{
    /// <summary>
    ///     Convert a <see cref="ConfigureEffectsProfileResource" /> into a
    ///     <see cref="ConfigureEffectsProfileCommand" />.
    /// </summary>
    /// <param name="resource">
    ///     The <see cref="ConfigureEffectsProfileResource" /> to convert.
    /// </param>
    /// <param name="movieId">
    ///     The unique identifier of the movie, taken from the route.
    /// </param>
    /// <returns>
    ///     The resulting <see cref="ConfigureEffectsProfileCommand" />.
    /// </returns>
    public static ConfigureEffectsProfileCommand ToCommandFromResource(
        ConfigureEffectsProfileResource resource,
        int movieId)
    {
        return new ConfigureEffectsProfileCommand(movieId, resource.IntensityLevel);
    }
}