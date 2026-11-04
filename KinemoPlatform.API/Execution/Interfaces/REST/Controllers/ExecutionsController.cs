using System.Net.Mime;
using KinemoPlatform.API.Execution.Application.Services;
using KinemoPlatform.API.Execution.Domain.Model.Commands;
using KinemoPlatform.API.Execution.Domain.Model.Queries;
using KinemoPlatform.API.Execution.Interfaces.REST.Resources;
using KinemoPlatform.API.Execution.Interfaces.REST.Transform;
using KinemoPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Execution")]
public class ExecutionsController(
    IShowExecutionCommandService commandService,
    IShowExecutionQueryService queryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Start Execution")]
    [EndpointDescription("Start a new 4D show execution (US13).")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> StartExecution([FromBody] StartExecutionResource resource,
        CancellationToken cancellationToken)
    {
        var command = StartExecutionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromExecutionResultAssembler.ToActionResult(result, this, localizer,
            e => CreatedAtAction(nameof(GetExecutionById), new { executionId = e.Id },
                ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(e)));
    }

    [HttpGet("{executionId:int}")]
    [EndpointSummary("Get Execution by Id")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExecutionById([FromRoute] int executionId,
        CancellationToken cancellationToken)
    {
        var execution = await queryService.Handle(new GetExecutionByIdQuery(executionId), cancellationToken);
        if (execution is null) return NotFound();
        return Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
    }

    [HttpGet("show/{showId:int}")]
    [EndpointSummary("Get Active Execution by Show ID")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExecutionByShowId([FromRoute] int showId,
        CancellationToken cancellationToken)
    {
        var execution = await queryService.Handle(new GetExecutionByShowIdQuery(showId), cancellationToken);
        if (execution is null) return NotFound();
        return Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
    }

    [HttpPost("{executionId:int}/pause")]
    [EndpointSummary("Pause Execution")]
    [EndpointDescription("Pause an active execution (US16).")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PauseExecution([FromRoute] int executionId,
        CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new PauseExecutionCommand(executionId), cancellationToken);
        return ActionResultFromExecutionResultAssembler.ToActionResult(result, this, localizer,
            e => Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(e)));
    }

    [HttpPost("{executionId:int}/resume")]
    [EndpointSummary("Resume Execution")]
    [EndpointDescription("Resume a paused execution (US16).")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ResumeExecution([FromRoute] int executionId,
        CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new ResumeExecutionCommand(executionId), cancellationToken);
        return ActionResultFromExecutionResultAssembler.ToActionResult(result, this, localizer,
            e => Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(e)));
    }

    [HttpPost("{executionId:int}/finish")]
    [EndpointSummary("Finish Execution")]
    [EndpointDescription("Finish an execution normally (US15).")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> FinishExecution([FromRoute] int executionId,
        CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new FinishExecutionCommand(executionId), cancellationToken);
        return ActionResultFromExecutionResultAssembler.ToActionResult(result, this, localizer,
            e => Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(e)));
    }

    [HttpPost("{executionId:int}/emergency-stop")]
    [EndpointSummary("Emergency Stop")]
    [EndpointDescription("Trigger an emergency stop (US14).")]
    [ProducesResponseType(typeof(ShowExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> TriggerEmergencyStop([FromRoute] int executionId,
        [FromBody] TriggerEmergencyStopResource resource, CancellationToken cancellationToken)
    {
        var command = TriggerEmergencyStopCommandFromResourceAssembler.ToCommandFromResource(resource, executionId);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromExecutionResultAssembler.ToActionResult(result, this, localizer,
            e => Ok(ShowExecutionResourceFromEntityAssembler.ToResourceFromEntity(e)));
    }
}