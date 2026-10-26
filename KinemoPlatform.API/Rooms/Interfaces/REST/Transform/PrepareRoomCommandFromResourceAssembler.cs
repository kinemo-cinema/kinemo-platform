using KinemoPlatform.API.Rooms.Domain.Model.Commands;
using KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Transform;

public static class PrepareRoomCommandFromResourceAssembler
{
    public static PrepareRoomCommand ToCommandFromResource(PrepareRoomResource resource, int roomId)
    {
        return new PrepareRoomCommand(roomId, resource.Observations);
    }
}