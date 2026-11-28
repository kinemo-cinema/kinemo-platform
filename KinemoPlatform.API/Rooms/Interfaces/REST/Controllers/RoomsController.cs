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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomById([FromRoute] int roomId, CancellationToken cancellationToken)
    {
        var room = await roomQueryService.Handle(new GetRoomByIdQuery(roomId), cancellationToken);
        if (room is null)
            return Problem(title: localizer["RoomNotFound"].Value, statusCode: StatusCodes.Status404NotFound);
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
    [EndpointDescription("Perform a readiness inspection on the room. A blocked room must be released first.")]
    [ProducesResponseType(typeof(RoomResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [Obsolete("Use POST /api/v1/rooms/{roomId}/inspections")]
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
    [Obsolete("Use POST /api/v1/rooms/{roomId}/blocks")]
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
    [Obsolete("Use PATCH /api/v1/rooms/{roomId}/blocks/{blockId} with active=false")]
    public async Task<IActionResult> ReleaseRoom([FromRoute] int roomId, CancellationToken cancellationToken)
    {
        var result = await roomCommandService.Handle(new ReleaseRoomCommand(roomId), cancellationToken);
        return ActionResultFromRoomResultAssembler.ToActionResult(result, this, localizer,
            room => Ok(RoomResourceFromEntityAssembler.ToResourceFromEntity(room)));
    }
    
        // ---- Inspections ----

    [HttpPost("{roomId:int}/inspections")]
    [EndpointSummary("Create Room Inspection")]
    [EndpointDescription("Performs a readiness inspection. A blocked room must have its blocks lifted first.")]
    [ProducesResponseType(typeof(RoomInspectionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateInspection([FromRoute] int roomId, [FromBody] PrepareRoomResource resource,
        CancellationToken cancellationToken)
    {
        var result = await roomCommandService.Handle(
            new CreateRoomInspectionCommand(roomId, resource.Observations), cancellationToken);
        return ActionResultFromRoomResultAssembler.ToActionResult(result, this, localizer,
            inspection => CreatedAtAction(nameof(GetInspectionById),
                new { roomId, inspectionId = inspection.Id },
                RoomInspectionResourceFromEntityAssembler.ToResourceFromEntity(inspection)));
    }

    [HttpGet("{roomId:int}/inspections")]
    [EndpointSummary("List Room Inspections")]
    [ProducesResponseType(typeof(IEnumerable<RoomInspectionResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInspections([FromRoute] int roomId, CancellationToken cancellationToken)
    {
        var room = await roomQueryService.Handle(new GetRoomByIdQuery(roomId), cancellationToken);
        if (room is null)
            return Problem(title: localizer["RoomNotFound"].Value, statusCode: StatusCodes.Status404NotFound);
        return Ok(room.Inspections.Select(RoomInspectionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{roomId:int}/inspections/{inspectionId:int}")]
    [EndpointSummary("Get Room Inspection")]
    [ProducesResponseType(typeof(RoomInspectionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInspectionById([FromRoute] int roomId, [FromRoute] int inspectionId,
        CancellationToken cancellationToken)
    {
        var room = await roomQueryService.Handle(new GetRoomByIdQuery(roomId), cancellationToken);
        if (room is null)
            return Problem(title: localizer["RoomNotFound"].Value, statusCode: StatusCodes.Status404NotFound);

        var inspection = room.Inspections.FirstOrDefault(i => i.Id == inspectionId);
        if (inspection is null)
            return Problem(title: localizer["RoomInspectionNotFound"].Value, statusCode: StatusCodes.Status404NotFound);
        return Ok(RoomInspectionResourceFromEntityAssembler.ToResourceFromEntity(inspection));
    }

    // ---- Blocks ----

    [HttpPost("{roomId:int}/blocks")]
    [EndpointSummary("Create Room Block")]
    [EndpointDescription("Blocks the room for a period, for example for maintenance.")]
    [ProducesResponseType(typeof(RoomBlockResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBlock([FromRoute] int roomId, [FromBody] BlockRoomResource resource,
        CancellationToken cancellationToken)
    {
        var result = await roomCommandService.Handle(
            new CreateRoomBlockCommand(roomId, resource.Reason, resource.StartDateTime, resource.EndDateTime),
            cancellationToken);
        return ActionResultFromRoomResultAssembler.ToActionResult(result, this, localizer,
            block => CreatedAtAction(nameof(GetBlockById),
                new { roomId, blockId = block.Id },
                RoomBlockResourceFromEntityAssembler.ToResourceFromEntity(block)));
    }

    [HttpGet("{roomId:int}/blocks")]
    [EndpointSummary("List Room Blocks")]
    [EndpointDescription("Lists the blocks of the room, optionally only the active (or only the lifted) ones.")]
    [ProducesResponseType(typeof(IEnumerable<RoomBlockResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlocks([FromRoute] int roomId, [FromQuery] bool? active,
        CancellationToken cancellationToken)
    {
        var room = await roomQueryService.Handle(new GetRoomByIdQuery(roomId), cancellationToken);
        if (room is null)
            return Problem(title: localizer["RoomNotFound"].Value, statusCode: StatusCodes.Status404NotFound);

        var blocks = room.Blocks
            .Where(block => active is null || block.Active == active)
            .Select(RoomBlockResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(blocks);
    }

    [HttpGet("{roomId:int}/blocks/{blockId:int}")]
    [EndpointSummary("Get Room Block")]
    [ProducesResponseType(typeof(RoomBlockResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlockById([FromRoute] int roomId, [FromRoute] int blockId,
        CancellationToken cancellationToken)
    {
        var room = await roomQueryService.Handle(new GetRoomByIdQuery(roomId), cancellationToken);
        if (room is null)
            return Problem(title: localizer["RoomNotFound"].Value, statusCode: StatusCodes.Status404NotFound);

        var block = room.Blocks.FirstOrDefault(b => b.Id == blockId);
        if (block is null)
            return Problem(title: localizer["RoomBlockNotFound"].Value, statusCode: StatusCodes.Status404NotFound);
        return Ok(RoomBlockResourceFromEntityAssembler.ToResourceFromEntity(block));
    }

    [HttpPatch("{roomId:int}/blocks/{blockId:int}")]
    [EndpointSummary("Lift Room Block")]
    [EndpointDescription("Lifts a block (active=false). The block stays in the history. The room becomes available again when no active block remains.")]
    [ProducesResponseType(typeof(RoomBlockResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBlock([FromRoute] int roomId, [FromRoute] int blockId,
        [FromBody] UpdateRoomBlockResource resource, CancellationToken cancellationToken)
    {
        if (resource.Active)
            return Problem(title: localizer["RoomBlockCannotBeReactivated"].Value,
                statusCode: StatusCodes.Status400BadRequest);

        var result = await roomCommandService.Handle(new LiftRoomBlockCommand(roomId, blockId), cancellationToken);
        return ActionResultFromRoomResultAssembler.ToActionResult(result, this, localizer,
            block => Ok(RoomBlockResourceFromEntityAssembler.ToResourceFromEntity(block)));
    }
}