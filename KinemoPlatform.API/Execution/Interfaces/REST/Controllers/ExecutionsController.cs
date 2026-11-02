using System.Net.Mime;
using KinemoPlatform.API.Execution.Domain.Model.Commands;
using KinemoPlatform.API.Execution.Domain.Model.Queries;
using KinemoPlatform.API.Execution.Domain.Services;
using KinemoPlatform.API.Execution.Interfaces.REST.Resources;
using KinemoPlatform.API.Execution.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Execution")]
public class ExecutionsController(
    IShowExecutionCommandService commandService,
    IShowExecutionQueryService queryService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Start Execution")]
    [EndpointDescription("Start a new 4D show execution (US13).")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> StartExecution([FromBody] StartExecutionResource resource)
    {
        try
        {
            var command = StartExecutionCommandFromResourceAssembler.ToCommandFromResource(resource);
            var execution = await commandService.Handle(command);
            if (execution is null) return BadRequest();
            
            var executionResource = ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution);
            return CreatedAtAction(nameof(GetExecutionById), new { executionId = execution.Id }, executionResource);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("{executionId:int}")]
    [EndpointSummary("Get Execution by Id")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExecutionById([FromRoute] int executionId)
    {
        var execution = await queryService.Handle(new GetExecutionByIdQuery(executionId));
        if (execution is null) return NotFound();
        return Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
    }

    [HttpGet("show/{showId:int}")]
    [EndpointSummary("Get Active Execution by Show ID")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExecutionByShowId([FromRoute] int showId)
    {
        var execution = await queryService.Handle(new GetExecutionByShowIdQuery(showId));
        if (execution is null) return NotFound();
        return Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
    }

    [HttpPost("{executionId:int}/pause")]
    [EndpointSummary("Pause Execution")]
    [EndpointDescription("Pause an active execution (US16).")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PauseExecution([FromRoute] int executionId)
    {
        try
        {
            var execution = await commandService.Handle(new PauseExecutionCommand(executionId));
            if (execution is null) return NotFound();
            return Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{executionId:int}/resume")]
    [EndpointSummary("Resume Execution")]
    [EndpointDescription("Resume a paused execution (US16).")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResumeExecution([FromRoute] int executionId)
    {
        try
        {
            var execution = await commandService.Handle(new ResumeExecutionCommand(executionId));
            if (execution is null) return NotFound();
            return Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{executionId:int}/finish")]
    [EndpointSummary("Finish Execution")]
    [EndpointDescription("Finish an execution normally (US15).")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FinishExecution([FromRoute] int executionId)
    {
        try
        {
            var execution = await commandService.Handle(new FinishExecutionCommand(executionId));
            if (execution is null) return NotFound();
            return Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{executionId:int}/emergency-stop")]
    [EndpointSummary("Emergency Stop")]
    [EndpointDescription("Trigger an emergency stop (US14).")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TriggerEmergencyStop([FromRoute] int executionId, [FromBody] TriggerEmergencyStopResource resource)
    {
        try
        {
            var command = TriggerEmergencyStopCommandFromResourceAssembler.ToCommandFromResource(resource, executionId);
            var execution = await commandService.Handle(command);
            if (execution is null) return NotFound();
            return Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}