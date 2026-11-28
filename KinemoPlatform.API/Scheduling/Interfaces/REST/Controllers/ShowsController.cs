using System.Net.Mime;
using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Scheduling.Application.Services;
using KinemoPlatform.API.Scheduling.Domain.Model.Commands;
using KinemoPlatform.API.Scheduling.Domain.Model.Queries;
using KinemoPlatform.API.Scheduling.Domain.Model.ValueObjects;
using KinemoPlatform.API.Scheduling.Interfaces.REST.Resources;
using KinemoPlatform.API.Scheduling.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Scheduling.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Show")]
public class ShowsController(
    IShowCommandService showCommandService,
    IShowQueryService showQueryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    [HttpGet("{showId:int}")]
    [EndpointSummary("Get Show by Id")]
    [ProducesResponseType(typeof(ShowResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetShowById([FromRoute] int showId, CancellationToken cancellationToken)
    {
        var show = await showQueryService.Handle(new GetShowByIdQuery(showId), cancellationToken);
        if (show is null)
            return Problem(title: localizer["ShowNotFound"].Value, statusCode: StatusCodes.Status404NotFound);
        return Ok(ShowResourceFromEntityAssembler.ToResourceFromEntity(show));
    }

    [HttpGet]
    [EndpointSummary("Get All Shows")]
    [EndpointDescription("Lists shows. All filters are optional and combine with AND. With a date the result is the daily schedule (US09), ordered by start time.")]
    [ProducesResponseType(typeof(IEnumerable<ShowResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllShows(
        [FromQuery] DateTimeOffset? date,
        [FromQuery] int? roomId,
        [FromQuery] int? movieId,
        [FromQuery] EShowStatus? status,
        CancellationToken cancellationToken)
    {
        var shows = await showQueryService.Handle(new GetShowsQuery(date, roomId, movieId, status), cancellationToken);
        return Ok(shows.Select(ShowResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("daily")]
    [EndpointSummary("Get Daily Schedule")]
    [EndpointDescription("Get all shows for a specific date (US09).")]
    [ProducesResponseType(typeof(IEnumerable<ShowResource>), StatusCodes.Status200OK)]
    [Obsolete("Use GET /api/v1/shows?date=")]
    public async Task<IActionResult> GetDailySchedule([FromQuery] DateTimeOffset date,
        CancellationToken cancellationToken)
    {
        var shows = await showQueryService.Handle(new GetDailyScheduleQuery(date), cancellationToken);
        return Ok(shows.Select(ShowResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [EndpointSummary("Schedule Show")]
    [EndpointDescription("Program a new show (US07).")]
    [ProducesResponseType(typeof(ShowResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ScheduleShow([FromBody] ScheduleShowResource resource,
        CancellationToken cancellationToken)
    {
        var command = ScheduleShowCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await showCommandService.Handle(command, cancellationToken);
        return ActionResultFromShowResultAssembler.ToActionResult(result, this, localizer,
            show => CreatedAtAction(nameof(GetShowById), new { showId = show.Id },
                ShowResourceFromEntityAssembler.ToResourceFromEntity(show)));
    }

    [HttpPatch("{showId:int}")]
    [EndpointSummary("Edit Show")]
    [EndpointDescription("Update the time slot of an existing show (US08).")]
    [ProducesResponseType(typeof(ShowResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> EditShow([FromRoute] int showId, [FromBody] EditShowResource resource,
        CancellationToken cancellationToken)
    {
        var command = EditShowCommandFromResourceAssembler.ToCommandFromResource(resource, showId);
        var result = await showCommandService.Handle(command, cancellationToken);
        return ActionResultFromShowResultAssembler.ToActionResult(result, this, localizer,
            show => Ok(ShowResourceFromEntityAssembler.ToResourceFromEntity(show)));
    }

    [HttpPost("{showId:int}/cancellation")]
    [EndpointSummary("Cancel Show")]
    [EndpointDescription("Cancel a scheduled show (US08).")]
    [ProducesResponseType(typeof(ShowResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelShow([FromRoute] int showId, CancellationToken cancellationToken)
    {
        var result = await showCommandService.Handle(new CancelShowCommand(showId), cancellationToken);
        return ActionResultFromShowResultAssembler.ToActionResult(result, this, localizer,
            show => Ok(ShowResourceFromEntityAssembler.ToResourceFromEntity(show)));
    }
}