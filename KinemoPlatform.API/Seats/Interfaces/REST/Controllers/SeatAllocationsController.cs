using System.Net.Mime;
using KinemoPlatform.API.Seats.Domain.Model.Queries;
using KinemoPlatform.API.Seats.Domain.Services;
using KinemoPlatform.API.Seats.Interfaces.REST.Resources;
using KinemoPlatform.API.Seats.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Seat Allocation")]
public class SeatAllocationsController(
    ISeatAllocationCommandService commandService,
    ISeatAllocationQueryService queryService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Initialize Seats")]
    [EndpointDescription("Generate the seat map for a specific show.")]
    [ProducesResponseType(typeof(SeatAllocationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> InitializeSeats([FromBody] InitializeSeatsResource resource)
    {
        try
        {
            var command = InitializeSeatsCommandFromResourceAssembler.ToCommandFromResource(resource);
            var allocation = await commandService.Handle(command);
            if (allocation is null) return BadRequest();
            
            var allocationResource = SeatAllocationResourceFromEntityAssembler.ToResourceFromEntity(allocation);
            return CreatedAtAction(nameof(GetAllocationByShowId), new { showId = allocation.ShowId }, allocationResource);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("{showId:int}")]
    [EndpointSummary("Get Seat Allocation by Show ID")]
    [ProducesResponseType(typeof(SeatAllocationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllocationByShowId([FromRoute] int showId)
    {
        var allocation = await queryService.Handle(new GetSeatAllocationByShowIdQuery(showId));
        if (allocation is null) return NotFound();
        return Ok(SeatAllocationResourceFromEntityAssembler.ToResourceFromEntity(allocation));
    }

    [HttpGet("{showId:int}/seats/{seatNumber:int}")]
    [EndpointSummary("Get Specific Seat")]
    [ProducesResponseType(typeof(ShowSeatResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetShowSeat([FromRoute] int showId, [FromRoute] int seatNumber)
    {
        var seat = await queryService.Handle(new GetShowSeatQuery(showId, seatNumber));
        if (seat is null) return NotFound();
        return Ok(ShowSeatResourceFromEntityAssembler.ToResourceFromEntity(seat));
    }

    [HttpPatch("{showId:int}/seats/{seatNumber:int}")]
    [EndpointSummary("Assign Seat Status")]
    [EndpointDescription("Manually change a seat's status (e.g., mark as Sold, Enabled, Disabled).")]
    [ProducesResponseType(typeof(SeatAllocationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignSeat([FromRoute] int showId, [FromRoute] int seatNumber, [FromBody] AssignSeatResource resource)
    {
        try
        {
            var command = AssignSeatCommandFromResourceAssembler.ToCommandFromResource(resource, showId, seatNumber);
            var allocation = await commandService.Handle(command);
            if (allocation is null) return NotFound();
            return Ok(SeatAllocationResourceFromEntityAssembler.ToResourceFromEntity(allocation));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpPost("{showId:int}/seats/{seatNumber:int}/out-of-service")]
    [EndpointSummary("Mark Seat Out of Service")]
    [EndpointDescription("Log a broken seat so it cannot be used (US12).")]
    [ProducesResponseType(typeof(SeatAllocationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkSeatOutOfService([FromRoute] int showId, [FromRoute] int seatNumber, [FromBody] MarkSeatOutOfServiceResource resource)
    {
        try
        {
            var command = MarkSeatOutOfServiceCommandFromResourceAssembler.ToCommandFromResource(resource, showId, seatNumber);
            var allocation = await commandService.Handle(command);
            if (allocation is null) return NotFound();
            return Ok(SeatAllocationResourceFromEntityAssembler.ToResourceFromEntity(allocation));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}