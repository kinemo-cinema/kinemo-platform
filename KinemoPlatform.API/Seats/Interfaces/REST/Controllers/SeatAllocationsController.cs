using System.Net.Mime;
using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Seats.Application.Services;
using KinemoPlatform.API.Seats.Domain.Model.Queries;
using KinemoPlatform.API.Seats.Interfaces.REST.Resources;
using KinemoPlatform.API.Seats.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Seat Allocation")]
public class SeatAllocationsController(
    ISeatAllocationCommandService commandService,
    ISeatAllocationQueryService queryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Initialize Seats")]
    [EndpointDescription("Generate the seat map for a specific show.")]
    [ProducesResponseType(typeof(SeatAllocationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> InitializeSeats(
        [FromBody] InitializeSeatsResource resource,
        CancellationToken cancellationToken)
    {
        var command = InitializeSeatsCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command, cancellationToken);

        return ActionResultFromSeatAllocationResultAssembler.ToActionResult(
            result,
            this,
            localizer,
            allocation => CreatedAtAction(
                nameof(GetAllocationByShowId),
                new { showId = allocation.ShowId },
                SeatAllocationResourceFromEntityAssembler.ToResourceFromEntity(allocation)));
    }

    [HttpGet("{showId:int}")]
    [EndpointSummary("Get Seat Allocation by Show ID")]
    [ProducesResponseType(typeof(SeatAllocationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllocationByShowId(
        [FromRoute] int showId,
        CancellationToken cancellationToken)
    {
        var allocation = await queryService.Handle(
            new GetSeatAllocationByShowIdQuery(showId),
            cancellationToken);

        if (allocation is null) return NotFound();

        return Ok(SeatAllocationResourceFromEntityAssembler.ToResourceFromEntity(allocation));
    }

    [HttpGet("{showId:int}/seats/{seatNumber:int}")]
    [EndpointSummary("Get Specific Seat")]
    [ProducesResponseType(typeof(ShowSeatResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetShowSeat(
        [FromRoute] int showId,
        [FromRoute] int seatNumber,
        CancellationToken cancellationToken)
    {
        var seat = await queryService.Handle(
            new GetShowSeatQuery(showId, seatNumber),
            cancellationToken);

        if (seat is null) return NotFound();

        return Ok(ShowSeatResourceFromEntityAssembler.ToResourceFromEntity(seat));
    }

    [HttpPatch("{showId:int}/seats/{seatNumber:int}")]
    [EndpointSummary("Assign Seat Status")]
    [EndpointDescription("Manually change a seat's status (e.g., mark as Sold, Enabled, Disabled).")]
    [ProducesResponseType(typeof(SeatAllocationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignSeat(
        [FromRoute] int showId,
        [FromRoute] int seatNumber,
        [FromBody] AssignSeatResource resource,
        CancellationToken cancellationToken)
    {
        var command = AssignSeatCommandFromResourceAssembler.ToCommandFromResource(
            resource, showId, seatNumber);

        var result = await commandService.Handle(command, cancellationToken);

        return ActionResultFromSeatAllocationResultAssembler.ToActionResult(
            result,
            this,
            localizer,
            allocation => Ok(SeatAllocationResourceFromEntityAssembler.ToResourceFromEntity(allocation)));
    }

    [HttpPost("{showId:int}/seats/{seatNumber:int}/out-of-service")]
    [EndpointSummary("Mark Seat Out of Service")]
    [EndpointDescription("Log a broken seat so it cannot be used (US12).")]
    [ProducesResponseType(typeof(SeatAllocationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkSeatOutOfService(
        [FromRoute] int showId,
        [FromRoute] int seatNumber,
        [FromBody] MarkSeatOutOfServiceResource resource,
        CancellationToken cancellationToken)
    {
        var command = MarkSeatOutOfServiceCommandFromResourceAssembler.ToCommandFromResource(
            resource, showId, seatNumber);

        var result = await commandService.Handle(command, cancellationToken);

        return ActionResultFromSeatAllocationResultAssembler.ToActionResult(
            result,
            this,
            localizer,
            allocation => Ok(SeatAllocationResourceFromEntityAssembler.ToResourceFromEntity(allocation)));
    }
}
