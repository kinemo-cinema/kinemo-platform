using System.Net.Mime;
using KinemoPlatform.API.Movies.Domain.Model.Commands;
using KinemoPlatform.API.Movies.Domain.Model.Queries;              // ← ADDED
using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;
using KinemoPlatform.API.Movies.Domain.Services;
using KinemoPlatform.API.Movies.Infrastructure.External.FileStorage;
using KinemoPlatform.API.Movies.Interfaces.REST.Resources;
using KinemoPlatform.API.Movies.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Http;                                    // ← ADDED
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Movie Endpoints.")]
public class MoviesController(
    IMovieCommandService movieCommandService,
    IMovieQueryService movieQueryService,
    ISensoryFileStorage sensoryFileStorage) : ControllerBase
{
    // ---------------------------------------------------------------------
    // GET /api/v1/movies/{movieId}
    // ---------------------------------------------------------------------
    [HttpGet("{movieId:int}")]
    [SwaggerOperation(
        Summary = "Get Movie by Id",
        Description = "Get a movie by its unique identifier.",
        OperationId = "GetMovieById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The movie was found.", typeof(MovieResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The movie was not found.")]
    public async Task<IActionResult> GetMovieById([FromRoute] int movieId)
    {
        var movie = await movieQueryService.Handle(new GetMovieByIdQuery(movieId));
        if (movie is null) return NotFound();
        var resource = MovieResourceFromEntityAssembler.ToResourceFromEntity(movie);
        return Ok(resource);
    }

    // ---------------------------------------------------------------------
    // GET /api/v1/movies
    // ---------------------------------------------------------------------
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Movies",
        Description = "Get all movies registered in the catalog.",
        OperationId = "GetAllMovies")]
    [SwaggerResponse(StatusCodes.Status200OK, "The movies were found.", typeof(IEnumerable<MovieResource>))]
    public async Task<IActionResult> GetAllMovies()
    {
        var movies = await movieQueryService.Handle(new GetAllMoviesQuery());
        var resources = movies.Select(MovieResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    // ---------------------------------------------------------------------
    // GET /api/v1/movies/search?title=...&genre=...
    // ---------------------------------------------------------------------
    [HttpGet("search")]
    [SwaggerOperation(
        Summary = "Search Movies",
        Description = "Search movies by optional partial title and/or genre.",
        OperationId = "SearchMovies")]
    [SwaggerResponse(StatusCodes.Status200OK, "The matching movies were found.", typeof(IEnumerable<MovieResource>))]
    public async Task<IActionResult> SearchMovies(
        [FromQuery] string? title,
        [FromQuery] EMovieGenre? genre)
    {
        var movies = await movieQueryService.Handle(new SearchMoviesQuery(title, genre));
        var resources = movies.Select(MovieResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    // ---------------------------------------------------------------------
    // POST /api/v1/movies
    // ---------------------------------------------------------------------
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Movie",
        Description = "Register a new 4D movie in the platform.",
        OperationId = "CreateMovie")]
    [SwaggerResponse(StatusCodes.Status201Created, "The movie was created.", typeof(MovieResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body is invalid.")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "A movie with the same title already exists.")]
    public async Task<IActionResult> CreateMovie([FromBody] CreateMovieResource resource)
    {
        try
        {
            var command = CreateMovieCommandFromResourceAssembler.ToCommandFromResource(resource);
            var movie = await movieCommandService.Handle(command);
            if (movie is null) return BadRequest();
            var movieResource = MovieResourceFromEntityAssembler.ToResourceFromEntity(movie);
            return CreatedAtAction(nameof(GetMovieById), new { movieId = movie.Id }, movieResource);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // ---------------------------------------------------------------------
    // PATCH /api/v1/movies/{movieId}
    // ---------------------------------------------------------------------
    [HttpPatch("{movieId:int}")]
    [SwaggerOperation(
        Summary = "Update Movie",
        Description = "Update the basic information of an existing movie.",
        OperationId = "UpdateMovie")]
    [SwaggerResponse(StatusCodes.Status200OK, "The movie was updated.", typeof(MovieResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The movie was not found.")]
    public async Task<IActionResult> UpdateMovie(
        [FromRoute] int movieId,
        [FromBody] UpdateMovieResource resource)
    {
        try
        {
            var command = UpdateMovieCommandFromResourceAssembler.ToCommandFromResource(resource, movieId);
            var movie = await movieCommandService.Handle(command);
            if (movie is null) return NotFound();
            var movieResource = MovieResourceFromEntityAssembler.ToResourceFromEntity(movie);
            return Ok(movieResource);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    // ---------------------------------------------------------------------
    // POST /api/v1/movies/{movieId}/deactivation
    // ---------------------------------------------------------------------
    [HttpPost("{movieId:int}/deactivation")]
    [SwaggerOperation(
        Summary = "Deactivate Movie",
        Description = "Mark a movie as inactive so it cannot be scheduled in new shows (US04).",
        OperationId = "DeactivateMovie")]
    [SwaggerResponse(StatusCodes.Status200OK, "The movie was deactivated.", typeof(MovieResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The movie was not found.")]
    public async Task<IActionResult> DeactivateMovie([FromRoute] int movieId)
    {
        try
        {
            var command = new DeactivateMovieCommand(movieId);
            var movie = await movieCommandService.Handle(command);
            if (movie is null) return NotFound();
            var movieResource = MovieResourceFromEntityAssembler.ToResourceFromEntity(movie);
            return Ok(movieResource);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    // ---------------------------------------------------------------------
    // GET /api/v1/movies/{movieId}/sensory-content
    // ---------------------------------------------------------------------
    [HttpGet("{movieId:int}/sensory-content")]
    [SwaggerOperation(
        Summary = "Get Sensory Content",
        Description = "Get the sensory content configuration associated with a movie.",
        OperationId = "GetSensoryContentByMovieId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The sensory content was found.", typeof(SensoryContentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The movie or sensory content was not found.")]
    public async Task<IActionResult> GetSensoryContent([FromRoute] int movieId)
    {
        var sensoryContent = await movieQueryService.Handle(new GetSensoryContentByMovieIdQuery(movieId));
        if (sensoryContent is null) return NotFound();
        var resource = SensoryContentResourceFromEntityAssembler.ToResourceFromEntity(sensoryContent);
        return Ok(resource);
    }

    // ---------------------------------------------------------------------
    // POST /api/v1/movies/{movieId}/sensory-file
    // ---------------------------------------------------------------------
    [HttpPost("{movieId:int}/sensory-file")]
    [Consumes("multipart/form-data")]
    [SwaggerOperation(
        Summary = "Associate Sensory File",
        Description = "Upload a sensory file and associate it with a movie's sensory content (US02).",
        OperationId = "AssociateSensoryFile")]
    [SwaggerResponse(StatusCodes.Status201Created, "The sensory file was associated.", typeof(MovieResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The movie was not found.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The uploaded file is invalid.")]
    public async Task<IActionResult> AssociateSensoryFile(
        [FromRoute] int movieId,
        [FromForm] IFormFile? file,                                    // ← made nullable
        [FromForm] EEffectType effectType,
        [FromForm] int durationSeconds)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { message = "A non-empty sensory file is required." });

        try
        {
            await using var stream = file.OpenReadStream();
            var filePath = await sensoryFileStorage.UploadAsync(stream, file.FileName, movieId);

            var command = new AssociateSensoryFileCommand(movieId, effectType, filePath, durationSeconds);
            var movie = await movieCommandService.Handle(command);
            if (movie is null) return NotFound();

            var resource = MovieResourceFromEntityAssembler.ToResourceFromEntity(movie);
            return CreatedAtAction(nameof(GetMovieById), new { movieId = movie.Id }, resource);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    // ---------------------------------------------------------------------
    // POST /api/v1/movies/{movieId}/effects-profile
    // ---------------------------------------------------------------------
    [HttpPost("{movieId:int}/effects-profile")]
    [SwaggerOperation(
        Summary = "Configure Effects Profile",
        Description = "Change the sensory intensity level applied to a movie's effects (US06).",
        OperationId = "ConfigureEffectsProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "The effects profile was updated.", typeof(MovieResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The movie was not found.")]
    public async Task<IActionResult> ConfigureEffectsProfile(
        [FromRoute] int movieId,
        [FromBody] ConfigureEffectsProfileResource resource)
    {
        try
        {
            var command = ConfigureEffectsProfileCommandFromResourceAssembler
                .ToCommandFromResource(resource, movieId);
            var movie = await movieCommandService.Handle(command);
            if (movie is null) return NotFound();
            var movieResource = MovieResourceFromEntityAssembler.ToResourceFromEntity(movie);
            return Ok(movieResource);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}