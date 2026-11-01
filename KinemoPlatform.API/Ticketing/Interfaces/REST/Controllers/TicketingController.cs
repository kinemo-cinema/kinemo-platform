using System.Net.Mime;
using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Ticketing.Application.Services;
using KinemoPlatform.API.Ticketing.Domain.Model.Queries;
using KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;
using KinemoPlatform.API.Ticketing.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Ticketing Integration")]
public class TicketingController(
    ITicketingCommandService ticketingCommandService,
    ITicketingQueryService ticketingQueryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    [HttpPost("integrations")]
    [EndpointSummary("Create Integration")]
    [EndpointDescription("Register a new external ticketing integration.")]
    [ProducesResponseType(typeof(TicketingIntegrationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateIntegration([FromBody] CreateIntegrationResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateIntegrationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await ticketingCommandService.Handle(command, cancellationToken);
        return ActionResultFromTicketingResultAssembler.ToActionResult(result, this, localizer,
            integration => CreatedAtAction(nameof(GetIntegrationById), new { integrationId = integration.Id },
                TicketingIntegrationResourceFromEntityAssembler.ToResourceFromEntity(integration)));
    }

    [HttpGet("integrations/{integrationId:int}")]
    [EndpointSummary("Get Integration by Id")]
    [ProducesResponseType(typeof(TicketingIntegrationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIntegrationById([FromRoute] int integrationId,
        CancellationToken cancellationToken)
    {
        var integration = await ticketingQueryService.Handle(new GetIntegrationByIdQuery(integrationId),
            cancellationToken);
        if (integration is null) return NotFound();
        return Ok(TicketingIntegrationResourceFromEntityAssembler.ToResourceFromEntity(integration));
    }

    [HttpPost("integrations/{integrationId:int}/sync")]
    [EndpointSummary("Sync Show Occupancy")]
    [EndpointDescription("Fetch the latest occupancy data for a show from the external provider.")]
    [ProducesResponseType(typeof(TicketingIntegrationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SyncOccupancy([FromRoute] int integrationId,
        [FromBody] SyncOccupancyResource resource, CancellationToken cancellationToken)
    {
        var command = SyncOccupancyCommandFromResourceAssembler.ToCommandFromResource(resource, integrationId);
        var result = await ticketingCommandService.Handle(command, cancellationToken);
        return ActionResultFromTicketingResultAssembler.ToActionResult(result, this, localizer,
            integration => Ok(TicketingIntegrationResourceFromEntityAssembler.ToResourceFromEntity(integration)));
    }

    [HttpGet("occupancies/{showId:int}")]
    [EndpointSummary("Get Show Occupancy")]
    [EndpointDescription("Retrieve the current stored occupancy for a specific show.")]
    [ProducesResponseType(typeof(ShowOccupancyResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOccupancyByShowId([FromRoute] int showId, CancellationToken cancellationToken)
    {
        var occupancy = await ticketingQueryService.Handle(new GetOccupancyByShowIdQuery(showId), cancellationToken);
        if (occupancy is null) return NotFound();
        return Ok(ShowOccupancyResourceFromEntityAssembler.ToResourceFromEntity(occupancy));
    }
}