using System.Net.Mime;
using KinemoPlatform.API.Rooms.Domain.Model.Commands;
using KinemoPlatform.API.Rooms.Domain.Model.Queries;
using KinemoPlatform.API.Rooms.Domain.Services;
using KinemoPlatform.API.Rooms.Interfaces.REST.Resources;
using KinemoPlatform.API.Rooms.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Room")]
public class RoomsController(
    IRoomCommandService roomCommandService,
    IRoomQueryService roomQueryService) : ControllerBase
{
    [HttpGet("{roomId:int}")]
    [EndpointSummary("Get Room by Id")]
    [ProducesResponseType(typeof(RoomResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomById([FromRoute] int roomId)
    {
        var room = await roomQueryService.Handle(new GetRoomByIdQuery(roomId));
        if (room is null) return NotFound();
        return Ok(RoomResourceFromEntityAssembler.ToResourceFromEntity(room));
    }

    [HttpGet]
    [EndpointSummary("Get All Rooms")]
    [ProducesResponseType(typeof(IEnumerable<RoomResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await roomQueryService.Handle(new GetAllRoomsQuery());
        return Ok(rooms.Select(RoomResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [EndpointSummary("Create Room")]
    [ProducesResponseType(typeof(RoomResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomResource resource)
    {
        try
        {
            var command = CreateRoomCommandFromResourceAssembler.ToCommandFromResource(resource);
            var room = await roomCommandService.Handle(command);
            if (room is null) return BadRequest();
            
            var roomResource = RoomResourceFromEntityAssembler.ToResourceFromEntity(room);
            return CreatedAtAction(nameof(GetRoomById), new { roomId = room.Id }, roomResource);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("{roomId:int}/preparation")]
    [EndpointSummary("Prepare Room")]
    [EndpointDescription("Perform a readiness inspection on the room.")]
    [ProducesResponseType(typeof(RoomResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PrepareRoom([FromRoute] int roomId, [FromBody] PrepareRoomResource resource)
    {
        try
        {
            var command = PrepareRoomCommandFromResourceAssembler.ToCommandFromResource(resource, roomId);
            var room = await roomCommandService.Handle(command);
            if (room is null) return NotFound();
            return Ok(RoomResourceFromEntityAssembler.ToResourceFromEntity(room));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpPost("{roomId:int}/blocking")]
    [EndpointSummary("Block Room")]
    [EndpointDescription("Block a room for maintenance.")]
    [ProducesResponseType(typeof(RoomResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BlockRoom([FromRoute] int roomId, [FromBody] BlockRoomResource resource)
    {
        try
        {
            var command = BlockRoomCommandFromResourceAssembler.ToCommandFromResource(resource, roomId);
            var room = await roomCommandService.Handle(command);
            if (room is null) return NotFound();
            return Ok(RoomResourceFromEntityAssembler.ToResourceFromEntity(room));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpPost("{roomId:int}/release")]
    [EndpointSummary("Release Room")]
    [EndpointDescription("Release a room from maintenance.")]
    [ProducesResponseType(typeof(RoomResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReleaseRoom([FromRoute] int roomId)
    {
        try
        {
            var room = await roomCommandService.Handle(new ReleaseRoomCommand(roomId));
            if (room is null) return NotFound();
            return Ok(RoomResourceFromEntityAssembler.ToResourceFromEntity(room));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}