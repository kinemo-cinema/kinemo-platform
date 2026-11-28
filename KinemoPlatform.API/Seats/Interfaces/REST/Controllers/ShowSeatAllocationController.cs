using System.Net.Mime;
using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Seats.Application.Services;
using KinemoPlatform.API.Seats.Domain.Model.Commands;
using KinemoPlatform.API.Seats.Domain.Model.Queries;
using KinemoPlatform.API.Seats.Domain.Model.ValueObjects;
using KinemoPlatform.API.Seats.Interfaces.REST.Resources;
using KinemoPlatform.API.Seats.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Controllers;

/// <summary>The seat allocation of a show. A show has exactly one, so it is a singleton sub-resource.</summary>
[ApiController]
[Route("api/v1/shows/{showId:int}/seat-allocation")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Seat Allocation")]
public class ShowSeatAllocationController(
    ISeatAllocationCommandService commandService,
    ISeatAllocationQueryService queryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create Seat Allocation")]
    [EndpointDescription("Generates the seat map of a scheduled show from its room capacity.")]
    [ProducesResponseType(typeof(SeatAllocationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateSeatAllocation(
        [FromRoute] int showId,
        CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new InitializeSeatsCommand(showId), cancellationToken);
        return ActionResultFromSeatAllocationResultAssembler.ToActionResult(
            result,
            this,
            localizer,
            allocation => CreatedAtAction(
                nameof(GetSeatAllocation),
                new { showId = allocation.ShowId },
                SeatAllocationResourceFromEntityAssembler.ToResourceFromEntity(allocation)));
    }

    [HttpGet]
    [EndpointSummary("Get Seat Allocation")]
    [ProducesResponseType(typeof(SeatAllocationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSeatAllocation(
        [FromRoute] int showId,
        CancellationToken cancellationToken)
    {
        var allocation = await queryService.Handle(new GetSeatAllocationByShowIdQuery(showId), cancellationToken);
        if (allocation is null)
            return Problem(title: localizer["SeatAllocationNotFound"].Value, statusCode: StatusCodes.Status404NotFound);
        return Ok(SeatAllocationResourceFromEntityAssembler.ToResourceFromEntity(allocation));
    }

    [HttpGet("seats")]
    [EndpointSummary("List Seats")]
    [EndpointDescription("Lists the seats of the show, optionally filtered by status.")]
    [ProducesResponseType(typeof(IEnumerable<ShowSeatResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSeats(
        [FromRoute] int showId,
        [FromQuery] ESeatStatus? status,
        CancellationToken cancellationToken)
    {
        var allocation = await queryService.Handle(new GetSeatAllocationByShowIdQuery(showId), cancellationToken);
        if (allocation is null)
            return Problem(title: localizer["SeatAllocationNotFound"].Value, statusCode: StatusCodes.Status404NotFound);

        var seats = allocation.ShowSeats
            .Where(seat => status is null || seat.Status == status)
            .OrderBy(seat => seat.SeatNumber)
            .Select(ShowSeatResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(seats);
    }

    [HttpGet("seats/{seatNumber:int}")]
    [EndpointSummary("Get Seat")]
    [ProducesResponseType(typeof(ShowSeatResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSeat(
        [FromRoute] int showId,
        [FromRoute] int seatNumber,
        CancellationToken cancellationToken)
    {
        var seat = await queryService.Handle(new GetShowSeatQuery(showId, seatNumber), cancellationToken);
        if (seat is null)
            return Problem(title: localizer["SeatNotFound"].Value, statusCode: StatusCodes.Status404NotFound);
        return Ok(ShowSeatResourceFromEntityAssembler.ToResourceFromEntity(seat));
    }

    [HttpPatch("seats/{seatNumber:int}")]
    [EndpointSummary("Update Seat Status")]
    [EndpointDescription("Changes a seat's status (Enabled, Disabled, OutOfService, ...). Every change is recorded as a manual intervention.")]
    [ProducesResponseType(typeof(ShowSeatResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateSeat(
        [FromRoute] int showId,
        [FromRoute] int seatNumber,
        [FromBody] AssignSeatResource resource,
        CancellationToken cancellationToken)
    {
        var command = AssignSeatCommandFromResourceAssembler.ToCommandFromResource(resource, showId, seatNumber);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromSeatAllocationResultAssembler.ToActionResult(
            result,
            this,
            localizer,
            allocation => Ok(ShowSeatResourceFromEntityAssembler.ToResourceFromEntity(
                allocation.ShowSeats.First(seat => seat.SeatNumber == seatNumber))));
    }
}