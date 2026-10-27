using System.Net.Mime;
using KinemoPlatform.API.Maintenance.Domain.Services;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Maintenance Order Endpoints.")]
public class MaintenanceOrdersController(
    IMaintenanceOrderCommandService commandService,
    IMaintenanceOrderQueryService queryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Schedule Maintenance", Description = "Schedule preventive or corrective maintenance (US23).", OperationId = "ScheduleMaintenance")]
    [SwaggerResponse(StatusCodes.Status201Created, "The maintenance order was created.", typeof(MaintenanceOrderResource))]
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
    [SwaggerOperation(Summary = "Complete Maintenance", Description = "Mark an order as completed and resolve its incident (US21).", OperationId = "CompleteMaintenance")]
    [SwaggerResponse(StatusCodes.Status200OK, "The maintenance was completed.", typeof(MaintenanceOrderResource))]
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
    [SwaggerOperation(Summary = "Get Orders by Equipment", Description = "Get the maintenance history for a piece of equipment (US22).", OperationId = "GetOrdersByEquipmentId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The orders were found.", typeof(IEnumerable<MaintenanceOrderResource>))]
    public async Task<IActionResult> GetOrdersByEquipmentId([FromRoute] int equipmentId)
    {
        var orders = await queryService.HandleGetByEquipmentId(equipmentId);
        return Ok(orders.Select(MaintenanceOrderResourceFromEntityAssembler.ToResourceFromEntity));
    }
}