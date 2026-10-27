using System.Net.Mime;
using KinemoPlatform.API.Maintenance.Domain.Model.Queries;
using KinemoPlatform.API.Maintenance.Domain.Services;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Equipment Endpoints.")]
public class EquipmentController(
    IEquipmentCommandService commandService,
    IEquipmentQueryService queryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Register Equipment", Description = "Register a new hardware component in a room (US19).", OperationId = "RegisterEquipment")]
    [SwaggerResponse(StatusCodes.Status201Created, "The equipment was registered.", typeof(EquipmentResource))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Equipment with the same name already exists in this room.")]
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
    [SwaggerOperation(Summary = "Get All Equipment", OperationId = "GetAllEquipment")]
    [SwaggerResponse(StatusCodes.Status200OK, "The equipment was found.", typeof(IEnumerable<EquipmentResource>))]
    public async Task<IActionResult> GetAllEquipment()
    {
        var equipmentList = await queryService.Handle(new GetAllEquipmentQuery());
        return Ok(equipmentList.Select(EquipmentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{equipmentId:int}")]
    [SwaggerOperation(Summary = "Get Equipment by Id", OperationId = "GetEquipmentById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The equipment was found.", typeof(EquipmentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The equipment was not found.")]
    public async Task<IActionResult> GetEquipmentById([FromRoute] int equipmentId)
    {
        var equipment = await queryService.Handle(new GetEquipmentByIdQuery(equipmentId));
        if (equipment is null) return NotFound();
        return Ok(EquipmentResourceFromEntityAssembler.ToResourceFromEntity(equipment));
    }

    [HttpPost("{equipmentId:int}/incidents")]
    [SwaggerOperation(Summary = "Report Incident", Description = "Report a broken component (US20).", OperationId = "ReportIncident")]
    [SwaggerResponse(StatusCodes.Status200OK, "The incident was registered.", typeof(EquipmentResource))]
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
    [SwaggerOperation(Summary = "Run Test", Description = "Run a diagnostic test on a specific channel (US17).", OperationId = "RunEquipmentTest")]
    [SwaggerResponse(StatusCodes.Status200OK, "The test was executed.", typeof(EquipmentResource))]
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