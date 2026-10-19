using System.Net.Mime;
using KinemoPlatform.API.Scheduling.Domain.Model.Commands;
using KinemoPlatform.API.Scheduling.Domain.Model.Queries;
using KinemoPlatform.API.Scheduling.Domain.Services;
using KinemoPlatform.API.Scheduling.Interfaces.REST.Resources;
using KinemoPlatform.API.Scheduling.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace KinemoPlatform.API.Scheduling.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Show Endpoints.")]
public class ShowsController(
    IShowCommandService showCommandService,
    IShowQueryService showQueryService) : ControllerBase
{
    [HttpGet("{showId:int}")]
    [SwaggerOperation(Summary = "Get Show by Id", OperationId = "GetShowById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The show was found.", typeof(ShowResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The show was not found.")]
    public async Task<IActionResult> GetShowById([FromRoute] int showId)
    {
        var show = await showQueryService.Handle(new GetShowByIdQuery(showId));
        if (show is null) return NotFound();
        return Ok(ShowResourceFromEntityAssembler.ToResourceFromEntity(show));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get All Shows", OperationId = "GetAllShows")]
    [SwaggerResponse(StatusCodes.Status200OK, "The shows were found.", typeof(IEnumerable<ShowResource>))]
    public async Task<IActionResult> GetAllShows()
    {
        var shows = await showQueryService.Handle(new GetAllShowsQuery());
        return Ok(shows.Select(ShowResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("daily")]
    [SwaggerOperation(Summary = "Get Daily Schedule", Description = "Get all shows for a specific date (US09).", OperationId = "GetDailySchedule")]
    [SwaggerResponse(StatusCodes.Status200OK, "The daily schedule was found.", typeof(IEnumerable<ShowResource>))]
    public async Task<IActionResult> GetDailySchedule([FromQuery] DateTimeOffset date)
    {
        var shows = await showQueryService.Handle(new GetDailyScheduleQuery(date));
        return Ok(shows.Select(ShowResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Schedule Show", Description = "Program a new show (US07).", OperationId = "ScheduleShow")]
    [SwaggerResponse(StatusCodes.Status201Created, "The show was scheduled.", typeof(ShowResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The room is already booked for this time slot.")]
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
    [SwaggerOperation(Summary = "Edit Show", Description = "Update the time slot of an existing show (US08).", OperationId = "EditShow")]
    [SwaggerResponse(StatusCodes.Status200OK, "The show was updated.", typeof(ShowResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The show was not found.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The new time slot overlaps with another show.")]
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
    [SwaggerOperation(Summary = "Cancel Show", Description = "Cancel a scheduled show (US08).", OperationId = "CancelShow")]
    [SwaggerResponse(StatusCodes.Status200OK, "The show was cancelled.", typeof(ShowResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The show was not found.")]
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