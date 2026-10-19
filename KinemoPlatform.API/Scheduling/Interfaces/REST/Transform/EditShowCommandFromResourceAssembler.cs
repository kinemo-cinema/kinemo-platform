using KinemoPlatform.API.Scheduling.Domain.Model.Commands;
using KinemoPlatform.API.Scheduling.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Scheduling.Interfaces.REST.Transform;

public static class EditShowCommandFromResourceAssembler
{
    public static EditShowCommand ToCommandFromResource(EditShowResource resource, int showId)
    {
        return new EditShowCommand(showId, resource.StartDateTime, resource.EndDateTime);
    }
}