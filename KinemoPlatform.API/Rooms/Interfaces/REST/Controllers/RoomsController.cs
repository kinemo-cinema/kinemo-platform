using System.Net.Mime;
using KinemoPlatform.API.Rooms.Domain.Model.Commands;
using KinemoPlatform.API.Rooms.Domain.Model.Queries;
using KinemoPlatform.API.Rooms.Domain.Services;
using KinemoPlatform.API.Rooms.Interfaces.REST.Resources;
using KinemoPlatform.API.Rooms.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Room Endpoints.")]
public class RoomsController(
    IRoomCommandService roomCommandService,
    IRoomQueryService roomQueryService) : ControllerBase
{
    [HttpGet("{roomId:int}")]
    [SwaggerOperation(Summary = "Get Room by Id", OperationId = "GetRoomById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The room was found.", typeof(RoomResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The room was not found.")]
    public async Task<IActionResult> GetRoomById([FromRoute] int roomId)
    {
        var room = await roomQueryService.Handle(new GetRoomByIdQuery(roomId));
        if (room is null) return NotFound();
        return Ok(RoomResourceFromEntityAssembler.ToResourceFromEntity(room));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get All Rooms", OperationId = "GetAllRooms")]
    [SwaggerResponse(StatusCodes.Status200OK, "The rooms were found.", typeof(IEnumerable<RoomResource>))]
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await roomQueryService.Handle(new GetAllRoomsQuery());
        return Ok(rooms.Select(RoomResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create Room", OperationId = "CreateRoom")]
    [SwaggerResponse(StatusCodes.Status201Created, "The room was created.", typeof(RoomResource))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "A room with the same name already exists.")]
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
    [SwaggerOperation(Summary = "Prepare Room", Description = "Perform a readiness inspection on the room.", OperationId = "PrepareRoom")]
    [SwaggerResponse(StatusCodes.Status200OK, "The room was prepared.", typeof(RoomResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The room was not found.")]
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
    [SwaggerOperation(Summary = "Block Room", Description = "Block a room for maintenance.", OperationId = "BlockRoom")]
    [SwaggerResponse(StatusCodes.Status200OK, "The room was blocked.", typeof(RoomResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The room was not found.")]
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
    [SwaggerOperation(Summary = "Release Room", Description = "Release a room from maintenance.", OperationId = "ReleaseRoom")]
    [SwaggerResponse(StatusCodes.Status200OK, "The room was released.", typeof(RoomResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The room was not found.")]
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