using KinemoPlatform.API.Scheduling.Domain.Model.Commands;
using KinemoPlatform.API.Scheduling.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Scheduling.Interfaces.REST.Transform;

public static class ScheduleShowCommandFromResourceAssembler
{
    public static ScheduleShowCommand ToCommandFromResource(ScheduleShowResource resource)
    {
        return new ScheduleShowCommand(
            resource.MovieId,
            resource.RoomId,
            resource.StartDateTime,
            resource.EndDateTime);
    }
}