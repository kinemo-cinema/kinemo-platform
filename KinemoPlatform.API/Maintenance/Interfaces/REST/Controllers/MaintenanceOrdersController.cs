using System.Net.Mime;
using KinemoPlatform.API.Maintenance.Domain.Services;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Maintenance Order")]
public class MaintenanceOrdersController(
    IMaintenanceOrderCommandService commandService,
    IMaintenanceOrderQueryService queryService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Schedule Maintenance")]
    [EndpointDescription("Schedule preventive or corrective maintenance (US23).")]
    [ProducesResponseType(typeof(MaintenanceOrderResource), StatusCodes.Status201Created)]
    public async Task<IActionResult> ScheduleMaintenance([FromBody] ScheduleMaintenanceResource resource)
    {
        try
        {
            var command = ScheduleMaintenanceCommandFromResourceAssembler.ToCommandFromResource(resource);
            var order = await commandService.Handle(command);
            if (order is null) return BadRequest();
            
            var orderResource = MaintenanceOrderResourceFromEntityAssembler.ToResourceFromEntity(order);
            return CreatedAtAction(nameof(GetOrdersByEquipmentId), new { equipmentId = order.EquipmentId }, orderResource);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{orderId:int}/completion")]
    [EndpointSummary("Complete Maintenance")]
    [EndpointDescription("Mark an order as completed and resolve its incident (US21).")]
    [ProducesResponseType(typeof(MaintenanceOrderResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> CompleteMaintenance([FromRoute] int orderId, [FromBody] CompleteMaintenanceResource resource)
    {
        try
        {
            var command = CompleteMaintenanceCommandFromResourceAssembler.ToCommandFromResource(resource, orderId);
            var order = await commandService.Handle(command);
            if (order is null) return NotFound();
            return Ok(MaintenanceOrderResourceFromEntityAssembler.ToResourceFromEntity(order));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpGet("equipment/{equipmentId:int}")]
    [EndpointSummary("Get Orders by Equipment")]
    [EndpointDescription("Get the maintenance history for a piece of equipment (US22).")]
    [ProducesResponseType(typeof(IEnumerable<MaintenanceOrderResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrdersByEquipmentId([FromRoute] int equipmentId)
    {
        var orders = await queryService.HandleGetByEquipmentId(equipmentId);
        return Ok(orders.Select(MaintenanceOrderResourceFromEntityAssembler.ToResourceFromEntity));
    }
}