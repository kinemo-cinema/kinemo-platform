using System.Net.Mime;
using KinemoPlatform.API.Execution.Domain.Model.Commands;
using KinemoPlatform.API.Execution.Domain.Model.Queries;
using KinemoPlatform.API.Execution.Domain.Services;
using KinemoPlatform.API.Execution.Interfaces.REST.Resources;
using KinemoPlatform.API.Execution.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Execution Endpoints.")]
public class ExecutionsController(
    IShowExecutionCommandService commandService,
    IShowExecutionQueryService queryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Start Execution", Description = "Start a new 4D show execution (US13).", OperationId = "StartExecution")]
    [SwaggerResponse(StatusCodes.Status201Created, "The execution was started.", typeof(ShowExecutionResource))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "An active execution already exists for this show.")]
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
    [SwaggerOperation(Summary = "Get Execution by Id", OperationId = "GetExecutionById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The execution was found.", typeof(ShowExecutionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The execution was not found.")]
    public async Task<IActionResult> GetExecutionById([FromRoute] int executionId)
    {
        var execution = await queryService.Handle(new GetExecutionByIdQuery(executionId));
        if (execution is null) return NotFound();
        return Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
    }

    [HttpGet("show/{showId:int}")]
    [SwaggerOperation(Summary = "Get Active Execution by Show ID", OperationId = "GetExecutionByShowId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The execution was found.", typeof(ShowExecutionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No active execution found.")]
    public async Task<IActionResult> GetExecutionByShowId([FromRoute] int showId)
    {
        var execution = await queryService.Handle(new GetExecutionByShowIdQuery(showId));
        if (execution is null) return NotFound();
        return Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
    }

    [HttpPost("{executionId:int}/pause")]
    [SwaggerOperation(Summary = "Pause Execution", Description = "Pause an active execution (US16).", OperationId = "PauseExecution")]
    [SwaggerResponse(StatusCodes.Status200OK, "The execution was paused.", typeof(ShowExecutionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Execution is not in progress.")]
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
    [SwaggerOperation(Summary = "Resume Execution", Description = "Resume a paused execution (US16).", OperationId = "ResumeExecution")]
    [SwaggerResponse(StatusCodes.Status200OK, "The execution was resumed.", typeof(ShowExecutionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Execution is not paused.")]
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
    [SwaggerOperation(Summary = "Finish Execution", Description = "Finish an execution normally (US15).", OperationId = "FinishExecution")]
    [SwaggerResponse(StatusCodes.Status200OK, "The execution was finished.", typeof(ShowExecutionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Execution is already finished or stopped.")]
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
    [SwaggerOperation(Summary = "Emergency Stop", Description = "Trigger an emergency stop (US14).", OperationId = "TriggerEmergencyStop")]
    [SwaggerResponse(StatusCodes.Status200OK, "The execution was emergency stopped.", typeof(ShowExecutionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The execution was not found.")]
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