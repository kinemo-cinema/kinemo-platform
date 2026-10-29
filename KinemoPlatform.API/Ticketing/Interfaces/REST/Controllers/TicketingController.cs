using System.Net.Mime;
using KinemoPlatform.API.Ticketing.Domain.Model.Queries;
using KinemoPlatform.API.Ticketing.Domain.Services;
using KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;
using KinemoPlatform.API.Ticketing.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Ticketing Integration")]
public class TicketingController(
    ITicketingCommandService ticketingCommandService,
    ITicketingQueryService ticketingQueryService) : ControllerBase
{
    [HttpPost("integrations")]
    [EndpointSummary("Create Integration")]
    [EndpointDescription("Register a new external ticketing integration.")]
    [ProducesResponseType(typeof(TicketingIntegrationResource), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateIntegration([FromBody] CreateIntegrationResource resource)
    {
        var command = CreateIntegrationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var integration = await ticketingCommandService.Handle(command);
        if (integration is null) return BadRequest();
        
        var integrationResource = TicketingIntegrationResourceFromEntityAssembler.ToResourceFromEntity(integration);
        return CreatedAtAction(nameof(GetIntegrationById), new { integrationId = integration.Id }, integrationResource);
    }

    [HttpGet("integrations/{integrationId:int}")]
    [EndpointSummary("Get Integration by Id")]
    [ProducesResponseType(typeof(TicketingIntegrationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIntegrationById([FromRoute] int integrationId)
    {
        var integration = await ticketingQueryService.Handle(new GetIntegrationByIdQuery(integrationId));
        if (integration is null) return NotFound();
        return Ok(TicketingIntegrationResourceFromEntityAssembler.ToResourceFromEntity(integration));
    }

    [HttpPost("integrations/{integrationId:int}/sync")]
    [EndpointSummary("Sync Show Occupancy")]
    [EndpointDescription("Fetch the latest occupancy data for a show from the external provider.")]
    [ProducesResponseType(typeof(TicketingIntegrationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [EndpointSummary("Get Show Occupancy")]
    [EndpointDescription("Retrieve the current stored occupancy for a specific show.")]
    [ProducesResponseType(typeof(ShowOccupancyResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOccupancyByShowId([FromRoute] int showId)
    {
        var occupancy = await ticketingQueryService.Handle(new GetOccupancyByShowIdQuery(showId));
        if (occupancy is null) return NotFound();
        return Ok(ShowOccupancyResourceFromEntityAssembler.ToResourceFromEntity(occupancy));
    }
}