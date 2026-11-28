using System.Net.Mime;
using KinemoPlatform.API.Movies.Domain.Model.Commands;
using KinemoPlatform.API.Movies.Domain.Model.Queries;
using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;
using KinemoPlatform.API.Movies.Application.Services;
using KinemoPlatform.API.Movies.Infrastructure.External.FileStorage;
using KinemoPlatform.API.Movies.Interfaces.REST.Resources;
using KinemoPlatform.API.Movies.Interfaces.REST.Transform;
using KinemoPlatform.API.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Movies.Interfaces.REST.Controllers;

/// <summary>
///     Movies controller.
/// </summary>
/// <remarks>
///     Exposes the REST endpoints of the Movies bounded context (BC01):
///     movie catalog management and sensory content configuration.
/// </remarks>
/// <param name="movieCommandService">
///     The <see cref="IMovieCommandService" /> to handle command operations.
/// </param>
/// <param name="movieQueryService">
///     The <see cref="IMovieQueryService" /> to handle query operations.
/// </param>
/// <param name="sensoryFileStorage">
///     The <see cref="ISensoryFileStorage" /> to persist sensory file uploads.
/// </param>
/// <param name="localizer">
///     The <see cref="IStringLocalizer{SharedResource}" /> used for localized error messages.
/// </param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Movie")]
public class MoviesController(
    IMovieCommandService movieCommandService,
    IMovieQueryService movieQueryService,
    
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    // ---------------------------------------------------------------------
    // GET /api/v1/movies/{movieId}
    // ---------------------------------------------------------------------
    [HttpGet("{movieId:int}")]
    [EndpointSummary("Get Movie by Id")]
    [EndpointDescription("Get a movie by its unique identifier.")]
    [ProducesResponseType(typeof(MovieResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMovieById([FromRoute] int movieId, CancellationToken cancellationToken)
    {
        var movie = await movieQueryService.Handle(new GetMovieByIdQuery(movieId), cancellationToken);
        if (movie is null)
            return Problem(title: localizer["MovieNotFound"].Value, statusCode: StatusCodes.Status404NotFound);
        var resource = MovieResourceFromEntityAssembler.ToResourceFromEntity(movie);
        return Ok(resource);
    }

    // ---------------------------------------------------------------------
    // GET /api/v1/movies
    // ---------------------------------------------------------------------
    [HttpGet]
    [EndpointSummary("Get All Movies")]
    [EndpointDescription("Lists the movies in the catalog. Optionally filter by partial title and/or genre (US05).")]
    [ProducesResponseType(typeof(IEnumerable<MovieResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMovies(
        [FromQuery] string? title,
        [FromQuery] EMovieGenre? genre,
        CancellationToken cancellationToken)
    {
        // With no criteria the search returns every movie.
        var movies = await movieQueryService.Handle(new SearchMoviesQuery(title, genre), cancellationToken);
        return Ok(movies.Select(MovieResourceFromEntityAssembler.ToResourceFromEntity));
    }

    // ---------------------------------------------------------------------
    // GET /api/v1/movies/search?title=...&genre=...
    // ---------------------------------------------------------------------
    [HttpGet("genres")]
    [EndpointSummary("Get Movie Genres")]
    [ProducesResponseType(typeof(IEnumerable<MovieGenreResource>), StatusCodes.Status200OK)]
    public IActionResult GetGenres()
    {
        return Ok(Enum.GetValues<EMovieGenre>()
            .Select(genre => new MovieGenreResource((int)genre, genre.ToString(), string.Empty)));
    }

    [HttpGet("search")]
    [EndpointSummary("Search Movies")]
    [EndpointDescription("Search movies by optional partial title and/or genre.")]
    [ProducesResponseType(typeof(IEnumerable<MovieResource>), StatusCodes.Status200OK)]
    [Obsolete("Use GET /api/v1/movies?title=&genre=")]
    public async Task<IActionResult> SearchMovies(
        [FromQuery] string? title,
        [FromQuery] EMovieGenre? genre,
        CancellationToken cancellationToken)
    {
        var movies = await movieQueryService.Handle(new SearchMoviesQuery(title, genre), cancellationToken);
        var resources = movies.Select(MovieResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    // ---------------------------------------------------------------------
    // POST /api/v1/movies
    // ---------------------------------------------------------------------
    [HttpPost]
    [EndpointSummary("Create Movie")]
    [EndpointDescription("Register a new 4D movie in the platform.")]
    [ProducesResponseType(typeof(MovieResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateMovie([FromBody] CreateMovieResource resource, CancellationToken cancellationToken)
    {
        var command = CreateMovieCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await movieCommandService.Handle(command, cancellationToken);
        return ActionResultFromMovieResultAssembler.ToActionResult(result, this, localizer,
            movie => CreatedAtAction(nameof(GetMovieById), new { movieId = movie.Id },
                MovieResourceFromEntityAssembler.ToResourceFromEntity(movie)));
    }

    // ---------------------------------------------------------------------
    // PATCH /api/v1/movies/{movieId}
    // ---------------------------------------------------------------------
    [HttpPatch("{movieId:int}")]
    [EndpointSummary("Update Movie")]
    [EndpointDescription("Update the basic information of an existing movie.")]
    [ProducesResponseType(typeof(MovieResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateMovie(
        [FromRoute] int movieId,
        [FromBody] UpdateMovieResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateMovieCommandFromResourceAssembler.ToCommandFromResource(resource, movieId);
        var result = await movieCommandService.Handle(command, cancellationToken);
        return ActionResultFromMovieResultAssembler.ToActionResult(result, this, localizer,
            movie => Ok(MovieResourceFromEntityAssembler.ToResourceFromEntity(movie)));
    }

    // ---------------------------------------------------------------------
    // POST /api/v1/movies/{movieId}/deactivation
    // ---------------------------------------------------------------------
    [HttpPost("{movieId:int}/deactivation")]
    [EndpointSummary("Deactivate Movie")]
    [EndpointDescription("Mark a movie as inactive so it cannot be scheduled in new shows (US04).")]
    [ProducesResponseType(typeof(MovieResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateMovie([FromRoute] int movieId, CancellationToken cancellationToken)
    {
        var result = await movieCommandService.Handle(new DeactivateMovieCommand(movieId), cancellationToken);
        return ActionResultFromMovieResultAssembler.ToActionResult(result, this, localizer,
            movie => Ok(MovieResourceFromEntityAssembler.ToResourceFromEntity(movie)));
    }

    // ---------------------------------------------------------------------
    // GET /api/v1/movies/{movieId}/sensory-content
    // ---------------------------------------------------------------------
    [HttpGet("{movieId:int}/sensory-content")]
    [EndpointSummary("Get Sensory Content")]
    [EndpointDescription("Get the sensory content configuration associated with a movie.")]
    [ProducesResponseType(typeof(SensoryContentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSensoryContent([FromRoute] int movieId, CancellationToken cancellationToken)
    {
        var sensoryContent = await movieQueryService.Handle(new GetSensoryContentByMovieIdQuery(movieId), cancellationToken);
        if (sensoryContent is null)
            return Problem(title: localizer["MovieNotFound"].Value, statusCode: StatusCodes.Status404NotFound);
        var resource = SensoryContentResourceFromEntityAssembler.ToResourceFromEntity(sensoryContent);
        return Ok(resource);
    }

    // ---------------------------------------------------------------------
    // POST /api/v1/movies/{movieId}/sensory-file
    // ---------------------------------------------------------------------
    [HttpPost("{movieId:int}/sensory-file")]
    [Consumes("multipart/form-data")]
    [EndpointSummary("Associate Sensory File")]
    [EndpointDescription("Upload a sensory file and associate it with a movie's sensory content (US02).")]
    [ProducesResponseType(typeof(MovieResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [Obsolete("Use POST /api/v1/movies/{movieId}/sensory-content/tracks")]
    public async Task<IActionResult> AssociateSensoryFile(
        [FromRoute] int movieId,
        IFormFile? file,                                    // <-- REMOVED [FromForm] HERE
        [FromForm] EEffectType effectType,
        [FromForm] int durationSeconds,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return Problem(title: localizer["SensoryFileRequired"].Value, statusCode: StatusCodes.Status400BadRequest);

        await using var stream = file.OpenReadStream();
        var result = await movieCommandService.Handle(
            new UploadSensoryTrackCommand(movieId, effectType, durationSeconds, file.FileName, stream),
            cancellationToken);

        return ActionResultFromMovieResultAssembler.ToActionResult(result, this, localizer,
            movie => CreatedAtAction(nameof(GetMovieById), new { movieId = movie.Id },
                MovieResourceFromEntityAssembler.ToResourceFromEntity(movie)));
    }

    // ---------------------------------------------------------------------
    // POST /api/v1/movies/{movieId}/effects-profile
    // ---------------------------------------------------------------------
    [HttpPost("{movieId:int}/effects-profile")]
    [EndpointSummary("Configure Effects Profile")]
    [EndpointDescription("Change the sensory intensity level applied to a movie's effects (US06).")]
    [ProducesResponseType(typeof(MovieResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [Obsolete("Use PATCH /api/v1/movies/{movieId}/sensory-content")]
    public async Task<IActionResult> ConfigureEffectsProfile(
        [FromRoute] int movieId,
        [FromBody] ConfigureEffectsProfileResource resource,
        CancellationToken cancellationToken)
    {
        var command = ConfigureEffectsProfileCommandFromResourceAssembler.ToCommandFromResource(resource, movieId);
        var result = await movieCommandService.Handle(command, cancellationToken);
        return ActionResultFromMovieResultAssembler.ToActionResult(result, this, localizer,
            movie => Ok(MovieResourceFromEntityAssembler.ToResourceFromEntity(movie)));
    }
    
        // ---- Sensory content as a sub-resource ----

    [HttpPatch("{movieId:int}/sensory-content")]
    [EndpointSummary("Update Sensory Content")]
    [EndpointDescription("Changes the sensory intensity level applied to the movie's effects (US06).")]
    [ProducesResponseType(typeof(SensoryContentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSensoryContent(
        [FromRoute] int movieId,
        [FromBody] ConfigureEffectsProfileResource resource,
        CancellationToken cancellationToken)
    {
        var command = ConfigureEffectsProfileCommandFromResourceAssembler.ToCommandFromResource(resource, movieId);
        var result = await movieCommandService.Handle(command, cancellationToken);
        return ActionResultFromMovieResultAssembler.ToActionResult(result, this, localizer,
            movie => Ok(SensoryContentResourceFromEntityAssembler.ToResourceFromEntity(movie.SensoryContent)));
    }

    [HttpPost("{movieId:int}/sensory-content/tracks")]
    [Consumes("multipart/form-data")]
    [EndpointSummary("Create Sensory Track")]
    [EndpointDescription("Uploads a sensory file and adds it as a track of the movie's sensory content. One track per effect type (US02).")]
    [ProducesResponseType(typeof(SensoryTrackResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateSensoryTrack(
        [FromRoute] int movieId,
        IFormFile? file,
        [FromForm] EEffectType effectType,
        [FromForm] int durationSeconds,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return Problem(title: localizer["SensoryFileRequired"].Value, statusCode: StatusCodes.Status400BadRequest);

        await using var stream = file.OpenReadStream();
        var result = await movieCommandService.Handle(
            new UploadSensoryTrackCommand(movieId, effectType, durationSeconds, file.FileName, stream),
            cancellationToken);

        return ActionResultFromMovieResultAssembler.ToActionResult(result, this, localizer, movie =>
        {
            // One track per effect type, so the effect type identifies the track that was just added.
            var track = movie.SensoryContent.Tracks.First(t => t.EffectType == effectType);
            return CreatedAtAction(nameof(GetSensoryTrackById), new { movieId, trackId = track.Id },
                SensoryTrackResourceFromEntityAssembler.ToResourceFromEntity(track));
        });
    }

    [HttpGet("{movieId:int}/sensory-content/tracks")]
    [EndpointSummary("List Sensory Tracks")]
    [ProducesResponseType(typeof(IEnumerable<SensoryTrackResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSensoryTracks([FromRoute] int movieId, CancellationToken cancellationToken)
    {
        var sensoryContent = await movieQueryService.Handle(new GetSensoryContentByMovieIdQuery(movieId), cancellationToken);
        if (sensoryContent is null)
            return Problem(title: localizer["MovieNotFound"].Value, statusCode: StatusCodes.Status404NotFound);
        return Ok(sensoryContent.Tracks.Select(SensoryTrackResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{movieId:int}/sensory-content/tracks/{trackId:int}")]
    [EndpointSummary("Get Sensory Track")]
    [ProducesResponseType(typeof(SensoryTrackResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSensoryTrackById([FromRoute] int movieId, [FromRoute] int trackId,
        CancellationToken cancellationToken)
    {
        var sensoryContent = await movieQueryService.Handle(new GetSensoryContentByMovieIdQuery(movieId), cancellationToken);
        if (sensoryContent is null)
            return Problem(title: localizer["MovieNotFound"].Value, statusCode: StatusCodes.Status404NotFound);

        var track = sensoryContent.Tracks.FirstOrDefault(t => t.Id == trackId);
        if (track is null)
            return Problem(title: localizer["SensoryTrackNotFound"].Value, statusCode: StatusCodes.Status404NotFound);
        return Ok(SensoryTrackResourceFromEntityAssembler.ToResourceFromEntity(track));
    }
}
