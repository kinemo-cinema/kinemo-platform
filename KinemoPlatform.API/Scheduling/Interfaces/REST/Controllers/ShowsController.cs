using System.Net.Mime;
using KinemoPlatform.API.Scheduling.Application.Services;
using KinemoPlatform.API.Scheduling.Domain.Model.Commands;
using KinemoPlatform.API.Scheduling.Domain.Model.Queries;
using KinemoPlatform.API.Scheduling.Interfaces.REST.Resources;
using KinemoPlatform.API.Scheduling.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace KinemoPlatform.API.Scheduling.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Show")]
public class ShowsController(
    IShowCommandService showCommandService,
    IShowQueryService showQueryService) : ControllerBase
{
    [HttpGet("{showId:int}")]
    [EndpointSummary("Get Show by Id")]
    [ProducesResponseType(typeof(ShowResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetShowById([FromRoute] int showId)
    {
        var show = await showQueryService.Handle(new GetShowByIdQuery(showId));
        if (show is null) return NotFound();
        return Ok(ShowResourceFromEntityAssembler.ToResourceFromEntity(show));
    }

    [HttpGet]
    [EndpointSummary("Get All Shows")]
    [ProducesResponseType(typeof(IEnumerable<ShowResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllShows()
    {
        var shows = await showQueryService.Handle(new GetAllShowsQuery());
        return Ok(shows.Select(ShowResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("daily")]
    [EndpointSummary("Get Daily Schedule")]
    [EndpointDescription("Get all shows for a specific date (US09).")]
    [ProducesResponseType(typeof(IEnumerable<ShowResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDailySchedule([FromQuery] DateTimeOffset date)
    {
        var shows = await showQueryService.Handle(new GetDailyScheduleQuery(date));
        return Ok(shows.Select(ShowResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [EndpointSummary("Schedule Show")]
    [EndpointDescription("Program a new show (US07).")]
    [ProducesResponseType(typeof(ShowResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ScheduleShow([FromBody] ScheduleShowResource resource)
    {
        try
        {
            var command = ScheduleShowCommandFromResourceAssembler.ToCommandFromResource(resource);
            var show = await showCommandService.Handle(command);
            if (show is null) return BadRequest();
            
            var showResource = ShowResourceFromEntityAssembler.ToResourceFromEntity(show);
            return CreatedAtAction(nameof(GetShowById), new { showId = show.Id }, showResource);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{showId:int}")]
    [EndpointSummary("Edit Show")]
    [EndpointDescription("Update the time slot of an existing show (US08).")]
    [ProducesResponseType(typeof(ShowResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditShow([FromRoute] int showId, [FromBody] EditShowResource resource)
    {
        try
        {
            var command = EditShowCommandFromResourceAssembler.ToCommandFromResource(resource, showId);
            var show = await showCommandService.Handle(command);
            if (show is null) return NotFound();
            return Ok(ShowResourceFromEntityAssembler.ToResourceFromEntity(show));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{showId:int}/cancellation")]
    [EndpointSummary("Cancel Show")]
    [EndpointDescription("Cancel a scheduled show (US08).")]
    [ProducesResponseType(typeof(ShowResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelShow([FromRoute] int showId)
    {
        try
        {
            var show = await showCommandService.Handle(new CancelShowCommand(showId));
            if (show is null) return NotFound();
            return Ok(ShowResourceFromEntityAssembler.ToResourceFromEntity(show));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}