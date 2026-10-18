using KinemoPlatform.API.Movies.Domain.Model.Commands;
using KinemoPlatform.API.Movies.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Transform;

/// <summary>
///     Assembler to create a <see cref="CreateMovieCommand" /> from a
///     <see cref="CreateMovieResource" />.
/// </summary>
public static class CreateMovieCommandFromResourceAssembler
{
    /// <summary>
    ///     Convert a <see cref="CreateMovieResource" /> into a
    ///     <see cref="CreateMovieCommand" />.
    /// </summary>
    /// <param name="resource">
    ///     The <see cref="CreateMovieResource" /> to convert.
    /// </param>
    /// <returns>
    ///     The resulting <see cref="CreateMovieCommand" />.
    /// </returns>
    public static CreateMovieCommand ToCommandFromResource(CreateMovieResource resource)
    {
        return new CreateMovieCommand(
            resource.Title,
            resource.DurationMinutes,
            resource.Genre);
    }
}