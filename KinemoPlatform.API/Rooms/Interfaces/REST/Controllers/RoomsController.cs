using System.Net.Mime;
using KinemoPlatform.API.Resources;
using KinemoPlatform.API.Rooms.Application.Services;
using KinemoPlatform.API.Rooms.Domain.Model.Commands;
using KinemoPlatform.API.Rooms.Domain.Model.Queries;
using KinemoPlatform.API.Rooms.Interfaces.REST.Resources;
using KinemoPlatform.API.Rooms.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Room")]
public class RoomsController(
    IRoomCommandService roomCommandService,
    IRoomQueryService roomQueryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    [HttpGet("{roomId:int}")]
    [EndpointSummary("Get Room by Id")]
    [ProducesResponseType(typeof(RoomResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomById([FromRoute] int roomId, CancellationToken cancellationToken)
    {
        var room = await roomQueryService.Handle(new GetRoomByIdQuery(roomId), cancellationToken);
        if (room is null) return NotFound();
        return Ok(RoomResourceFromEntityAssembler.ToResourceFromEntity(room));
    }

    [HttpGet]
    [EndpointSummary("Get All Rooms")]
    [ProducesResponseType(typeof(IEnumerable<RoomResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRooms(CancellationToken cancellationToken)
    {
        var rooms = await roomQueryService.Handle(new GetAllRoomsQuery(), cancellationToken);
        return Ok(rooms.Select(RoomResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [EndpointSummary("Create Room")]
    [ProducesResponseType(typeof(RoomResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateRoomCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await roomCommandService.Handle(command, cancellationToken);
        return ActionResultFromRoomResultAssembler.ToActionResult(result, this, localizer,
            room => CreatedAtAction(nameof(GetRoomById), new { roomId = room.Id },
                RoomResourceFromEntityAssembler.ToResourceFromEntity(room)));
    }

    [HttpPost("{roomId:int}/preparation")]
    [EndpointSummary("Prepare Room")]
    [EndpointDescription("Perform a readiness inspection on the room.")]
    [ProducesResponseType(typeof(RoomResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PrepareRoom([FromRoute] int roomId, [FromBody] PrepareRoomResource resource,
        CancellationToken cancellationToken)
    {
        var command = PrepareRoomCommandFromResourceAssembler.ToCommandFromResource(resource, roomId);
        var result = await roomCommandService.Handle(command, cancellationToken);
        return ActionResultFromRoomResultAssembler.ToActionResult(result, this, localizer,
            room => Ok(RoomResourceFromEntityAssembler.ToResourceFromEntity(room)));
    }

    [HttpPost("{roomId:int}/blocking")]
    [EndpointSummary("Block Room")]
    [EndpointDescription("Block a room for maintenance.")]
    [ProducesResponseType(typeof(RoomResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BlockRoom([FromRoute] int roomId, [FromBody] BlockRoomResource resource,
        CancellationToken cancellationToken)
    {
        var command = BlockRoomCommandFromResourceAssembler.ToCommandFromResource(resource, roomId);
        var result = await roomCommandService.Handle(command, cancellationToken);
        return ActionResultFromRoomResultAssembler.ToActionResult(result, this, localizer,
            room => Ok(RoomResourceFromEntityAssembler.ToResourceFromEntity(room)));
    }

    [HttpPost("{roomId:int}/release")]
    [EndpointSummary("Release Room")]
    [EndpointDescription("Release a room from maintenance.")]
    [ProducesResponseType(typeof(RoomResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReleaseRoom([FromRoute] int roomId, CancellationToken cancellationToken)
    {
        var result = await roomCommandService.Handle(new ReleaseRoomCommand(roomId), cancellationToken);
        return ActionResultFromRoomResultAssembler.ToActionResult(result, this, localizer,
            room => Ok(RoomResourceFromEntityAssembler.ToResourceFromEntity(room)));
    }
}