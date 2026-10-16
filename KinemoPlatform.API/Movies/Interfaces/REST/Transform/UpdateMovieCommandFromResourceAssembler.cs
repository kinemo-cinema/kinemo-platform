using KinemoPlatform.API.Movies.Domain.Model.Commands;
using KinemoPlatform.API.Movies.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Transform;

/// <summary>
///     Assembler to create an <see cref="UpdateMovieCommand" /> from an
///     <see cref="UpdateMovieResource" /> and a movie id from the route.
/// </summary>
public static class UpdateMovieCommandFromResourceAssembler
{
    /// <summary>
    ///     Convert an <see cref="UpdateMovieResource" /> into an
    ///     <see cref="UpdateMovieCommand" />.
    /// </summary>
    /// <param name="resource">
    ///     The <see cref="UpdateMovieResource" /> to convert.
    /// </param>
    /// <param name="movieId">
    ///     The unique identifier of the movie, taken from the route.
    /// </param>
    /// <returns>
    ///     The resulting <see cref="UpdateMovieCommand" />.
    /// </returns>
    public static UpdateMovieCommand ToCommandFromResource(UpdateMovieResource resource, int movieId)
    {
        return new UpdateMovieCommand(
            movieId,
            resource.Title,
            resource.DurationMinutes,
            resource.Genre);
    }
}