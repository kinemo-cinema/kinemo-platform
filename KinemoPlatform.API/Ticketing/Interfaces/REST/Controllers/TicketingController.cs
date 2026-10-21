using System.Net.Mime;
using KinemoPlatform.API.Ticketing.Domain.Model.Queries;
using KinemoPlatform.API.Ticketing.Domain.Services;
using KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;
using KinemoPlatform.API.Ticketing.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Ticketing Integration Endpoints.")]
public class TicketingController(
    ITicketingCommandService ticketingCommandService,
    ITicketingQueryService ticketingQueryService) : ControllerBase
{
    [HttpPost("integrations")]
    [SwaggerOperation(Summary = "Create Integration", Description = "Register a new external ticketing integration.", OperationId = "CreateIntegration")]
    [SwaggerResponse(StatusCodes.Status201Created, "The integration was created.", typeof(TicketingIntegrationResource))]
    public async Task<IActionResult> CreateIntegration([FromBody] CreateIntegrationResource resource)
    {
        var command = CreateIntegrationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var integration = await ticketingCommandService.Handle(command);
        if (integration is null) return BadRequest();
        
        var integrationResource = TicketingIntegrationResourceFromEntityAssembler.ToResourceFromEntity(integration);
        return CreatedAtAction(nameof(GetIntegrationById), new { integrationId = integration.Id }, integrationResource);
    }

    [HttpGet("integrations/{integrationId:int}")]
    [SwaggerOperation(Summary = "Get Integration by Id", OperationId = "GetIntegrationById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The integration was found.", typeof(TicketingIntegrationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The integration was not found.")]
    public async Task<IActionResult> GetIntegrationById([FromRoute] int integrationId)
    {
        var integration = await ticketingQueryService.Handle(new GetIntegrationByIdQuery(integrationId));
        if (integration is null) return NotFound();
        return Ok(TicketingIntegrationResourceFromEntityAssembler.ToResourceFromEntity(integration));
    }

    [HttpPost("integrations/{integrationId:int}/sync")]
    [SwaggerOperation(Summary = "Sync Show Occupancy", Description = "Fetch the latest occupancy data for a show from the external provider.", OperationId = "SyncOccupancy")]
    [SwaggerResponse(StatusCodes.Status200OK, "The synchronization was executed (check logs for success/failure).", typeof(TicketingIntegrationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The integration was not found.")]
    public async Task<IActionResult> SyncOccupancy([FromRoute] int integrationId, [FromBody] SyncOccupancyResource resource)
    {
        try
        {
            var command = SyncOccupancyCommandFromResourceAssembler.ToCommandFromResource(resource, integrationId);
            var integration = await ticketingCommandService.Handle(command);
            if (integration is null) return NotFound();
            
            return Ok(TicketingIntegrationResourceFromEntityAssembler.ToResourceFromEntity(integration));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpGet("occupancies/{showId:int}")]
    [SwaggerOperation(Summary = "Get Show Occupancy", Description = "Retrieve the current stored occupancy for a specific show.", OperationId = "GetOccupancyByShowId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The occupancy was found.", typeof(ShowOccupancyResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The occupancy was not found.")]
    public async Task<IActionResult> GetOccupancyByShowId([FromRoute] int showId)
    {
        var occupancy = await ticketingQueryService.Handle(new GetOccupancyByShowIdQuery(showId));
        if (occupancy is null) return NotFound();
        return Ok(ShowOccupancyResourceFromEntityAssembler.ToResourceFromEntity(occupancy));
    }
}