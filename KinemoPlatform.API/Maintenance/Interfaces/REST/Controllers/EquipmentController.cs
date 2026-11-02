using System.Net.Mime;
using KinemoPlatform.API.Maintenance.Application.Services;
using KinemoPlatform.API.Maintenance.Domain.Model.Queries;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
using KinemoPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Equipment")]
public class EquipmentController(
    IEquipmentCommandService commandService,
    IEquipmentQueryService queryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Register Equipment")]
    [EndpointDescription("Register a new hardware component in a room (US19).")]
    [ProducesResponseType(typeof(EquipmentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterEquipment([FromBody] RegisterEquipmentResource resource,
        CancellationToken cancellationToken)
    {
        var command = RegisterEquipmentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromEquipmentResultAssembler.ToActionResult(result, this, localizer,
            equipment => CreatedAtAction(nameof(GetEquipmentById), new { equipmentId = equipment.Id },
                EquipmentResourceFromEntityAssembler.ToResourceFromEntity(equipment)));
    }

    [HttpGet]
    [EndpointSummary("Get All Equipment")]
    [ProducesResponseType(typeof(IEnumerable<EquipmentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEquipment(CancellationToken cancellationToken)
    {
        var equipmentList = await queryService.Handle(new GetAllEquipmentQuery(), cancellationToken);
        return Ok(equipmentList.Select(EquipmentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{equipmentId:int}")]
    [EndpointSummary("Get Equipment by Id")]
    [ProducesResponseType(typeof(EquipmentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEquipmentById([FromRoute] int equipmentId, CancellationToken cancellationToken)
    {
        var equipment = await queryService.Handle(new GetEquipmentByIdQuery(equipmentId), cancellationToken);
        if (equipment is null) return NotFound();
        return Ok(EquipmentResourceFromEntityAssembler.ToResourceFromEntity(equipment));
    }

    [HttpPost("{equipmentId:int}/incidents")]
    [EndpointSummary("Report Incident")]
    [EndpointDescription("Report a broken component (US20).")]
    [ProducesResponseType(typeof(EquipmentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReportIncident([FromRoute] int equipmentId,
        [FromBody] RegisterIncidentResource resource, CancellationToken cancellationToken)
    {
        var command = RegisterIncidentCommandFromResourceAssembler.ToCommandFromResource(resource, equipmentId);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromEquipmentResultAssembler.ToActionResult(result, this, localizer,
            equipment => Ok(EquipmentResourceFromEntityAssembler.ToResourceFromEntity(equipment)));
    }

    [HttpPost("{equipmentId:int}/tests")]
    [EndpointSummary("Run Test")]
    [EndpointDescription("Run a diagnostic test on a specific channel (US17).")]
    [ProducesResponseType(typeof(EquipmentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RunTest([FromRoute] int equipmentId,
        [FromBody] RunEquipmentTestResource resource, CancellationToken cancellationToken)
    {
        var command = RunEquipmentTestCommandFromResourceAssembler.ToCommandFromResource(resource, equipmentId);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromEquipmentResultAssembler.ToActionResult(result, this, localizer,
            equipment => Ok(EquipmentResourceFromEntityAssembler.ToResourceFromEntity(equipment)));
    }
}
