using KinemoPlatform.API.Rooms.Domain.Model.Commands;
using KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Transform;

public static class CreateRoomCommandFromResourceAssembler
{
    public static CreateRoomCommand ToCommandFromResource(CreateRoomResource resource)
    {
        return new CreateRoomCommand(resource.Name, resource.Capacity);
    }
}