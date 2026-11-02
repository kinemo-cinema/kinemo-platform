using System.Net.Mime;
using KinemoPlatform.API.Maintenance.Application.Services;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
using KinemoPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Maintenance Order")]
public class MaintenanceOrdersController(
    IMaintenanceOrderCommandService commandService,
    IMaintenanceOrderQueryService queryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Schedule Maintenance")]
    [EndpointDescription("Schedule preventive or corrective maintenance (US23).")]
    [ProducesResponseType(typeof(MaintenanceOrderResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ScheduleMaintenance([FromBody] ScheduleMaintenanceResource resource,
        CancellationToken cancellationToken)
    {
        var command = ScheduleMaintenanceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromMaintenanceOrderResultAssembler.ToActionResult(result, this, localizer,
            order => CreatedAtAction(nameof(GetOrdersByEquipmentId), new { equipmentId = order.EquipmentId },
                MaintenanceOrderResourceFromEntityAssembler.ToResourceFromEntity(order)));
    }

    [HttpPost("{orderId:int}/completion")]
    [EndpointSummary("Complete Maintenance")]
    [EndpointDescription("Mark an order as completed and resolve its incident (US21).")]
    [ProducesResponseType(typeof(MaintenanceOrderResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteMaintenance([FromRoute] int orderId,
        [FromBody] CompleteMaintenanceResource resource, CancellationToken cancellationToken)
    {
        var command = CompleteMaintenanceCommandFromResourceAssembler.ToCommandFromResource(resource, orderId);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromMaintenanceOrderResultAssembler.ToActionResult(result, this, localizer,
            order => Ok(MaintenanceOrderResourceFromEntityAssembler.ToResourceFromEntity(order)));
    }

    [HttpGet("equipment/{equipmentId:int}")]
    [EndpointSummary("Get Orders by Equipment")]
    [EndpointDescription("Get the maintenance history for a piece of equipment (US22).")]
    [ProducesResponseType(typeof(IEnumerable<MaintenanceOrderResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrdersByEquipmentId([FromRoute] int equipmentId,
        CancellationToken cancellationToken)
    {
        var orders = await queryService.HandleGetByEquipmentId(equipmentId, cancellationToken);
        return Ok(orders.Select(MaintenanceOrderResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
