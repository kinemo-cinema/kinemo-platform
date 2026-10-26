using KinemoPlatform.API.Rooms.Domain.Model.Commands;
using KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Transform;

public static class BlockRoomCommandFromResourceAssembler
{
    public static BlockRoomCommand ToCommandFromResource(BlockRoomResource resource, int roomId)
    {
        return new BlockRoomCommand(roomId, resource.Reason, resource.StartDateTime, resource.EndDateTime);
    }
}