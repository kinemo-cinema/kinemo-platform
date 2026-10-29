using System.Net.Mime;
using KinemoPlatform.API.Maintenance.Domain.Model.Queries;
using KinemoPlatform.API.Maintenance.Domain.Services;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Equipment")]
public class EquipmentController(
    IEquipmentCommandService commandService,
    IEquipmentQueryService queryService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Register Equipment")]
    [EndpointDescription("Register a new hardware component in a room (US19).")]
    [ProducesResponseType(typeof(EquipmentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterEquipment([FromBody] RegisterEquipmentResource resource)
    {
        try
        {
            var command = RegisterEquipmentCommandFromResourceAssembler.ToCommandFromResource(resource);
            var equipment = await commandService.Handle(command);
            if (equipment is null) return BadRequest();
            
            var equipmentResource = EquipmentResourceFromEntityAssembler.ToResourceFromEntity(equipment);
            return CreatedAtAction(nameof(GetEquipmentById), new { equipmentId = equipment.Id }, equipmentResource);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    [EndpointSummary("Get All Equipment")]
    [ProducesResponseType(typeof(IEnumerable<EquipmentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEquipment()
    {
        var equipmentList = await queryService.Handle(new GetAllEquipmentQuery());
        return Ok(equipmentList.Select(EquipmentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{equipmentId:int}")]
    [EndpointSummary("Get Equipment by Id")]
    [ProducesResponseType(typeof(EquipmentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEquipmentById([FromRoute] int equipmentId)
    {
        var equipment = await queryService.Handle(new GetEquipmentByIdQuery(equipmentId));
        if (equipment is null) return NotFound();
        return Ok(EquipmentResourceFromEntityAssembler.ToResourceFromEntity(equipment));
    }

    [HttpPost("{equipmentId:int}/incidents")]
    [EndpointSummary("Report Incident")]
    [EndpointDescription("Report a broken component (US20).")]
    [ProducesResponseType(typeof(EquipmentResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReportIncident([FromRoute] int equipmentId, [FromBody] RegisterIncidentResource resource)
    {
        try
        {
            var command = RegisterIncidentCommandFromResourceAssembler.ToCommandFromResource(resource, equipmentId);
            var equipment = await commandService.Handle(command);
            if (equipment is null) return NotFound();
            return Ok(EquipmentResourceFromEntityAssembler.ToResourceFromEntity(equipment));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpPost("{equipmentId:int}/tests")]
    [EndpointSummary("Run Test")]
    [EndpointDescription("Run a diagnostic test on a specific channel (US17).")]
    [ProducesResponseType(typeof(EquipmentResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> RunTest([FromRoute] int equipmentId, [FromBody] RunEquipmentTestResource resource)
    {
        try
        {
            var command = RunEquipmentTestCommandFromResourceAssembler.ToCommandFromResource(resource, equipmentId);
            var equipment = await commandService.Handle(command);
            if (equipment is null) return NotFound();
            return Ok(EquipmentResourceFromEntityAssembler.ToResourceFromEntity(equipment));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}