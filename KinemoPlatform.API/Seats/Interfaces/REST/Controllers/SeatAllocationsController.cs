using System.Net.Mime;
using KinemoPlatform.API.Seats.Domain.Model.Queries;
using KinemoPlatform.API.Seats.Domain.Services;
using KinemoPlatform.API.Seats.Interfaces.REST.Resources;
using KinemoPlatform.API.Seats.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Seat Allocation Endpoints.")]
public class SeatAllocationsController(
    ISeatAllocationCommandService commandService,
    ISeatAllocationQueryService queryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Initialize Seats", Description = "Generate the seat map for a specific show.", OperationId = "InitializeSeats")]
    [SwaggerResponse(StatusCodes.Status201Created, "The seat map was generated.", typeof(SeatAllocationResource))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "A seat allocation for this show already exists.")]
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
    [SwaggerOperation(Summary = "Get Seat Allocation by Show ID", OperationId = "GetAllocationByShowId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The seat map was found.", typeof(SeatAllocationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The seat map was not found.")]
    public async Task<IActionResult> GetAllocationByShowId([FromRoute] int showId)
    {
        var allocation = await queryService.Handle(new GetSeatAllocationByShowIdQuery(showId));
        if (allocation is null) return NotFound();
        return Ok(SeatAllocationResourceFromEntityAssembler.ToResourceFromEntity(allocation));
    }

    [HttpGet("{showId:int}/seats/{seatNumber:int}")]
    [SwaggerOperation(Summary = "Get Specific Seat", OperationId = "GetShowSeat")]
    [SwaggerResponse(StatusCodes.Status200OK, "The seat was found.", typeof(ShowSeatResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The seat was not found.")]
    public async Task<IActionResult> GetShowSeat([FromRoute] int showId, [FromRoute] int seatNumber)
    {
        var seat = await queryService.Handle(new GetShowSeatQuery(showId, seatNumber));
        if (seat is null) return NotFound();
        return Ok(ShowSeatResourceFromEntityAssembler.ToResourceFromEntity(seat));
    }

    [HttpPatch("{showId:int}/seats/{seatNumber:int}")]
    [SwaggerOperation(Summary = "Assign Seat Status", Description = "Manually change a seat's status (e.g., mark as Sold, Enabled, Disabled).", OperationId = "AssignSeat")]
    [SwaggerResponse(StatusCodes.Status200OK, "The seat status was updated.", typeof(SeatAllocationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The seat or allocation was not found.")]
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
    [SwaggerOperation(Summary = "Mark Seat Out of Service", Description = "Log a broken seat so it cannot be used (US12).", OperationId = "MarkSeatOutOfService")]
    [SwaggerResponse(StatusCodes.Status200OK, "The seat was marked out of service.", typeof(SeatAllocationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The seat or allocation was not found.")]
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