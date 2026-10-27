using KinemoPlatform.API.Analytics.Domain.Model.Entities;
using KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Analytics.Interfaces.REST.Transform;
public static class RoomPerformanceResourceFromEntityAssembler
{
    public static RoomPerformanceResource ToResourceFromEntity(RoomPerformance e) => 
        new(e.Id, e.RoomId, e.OperatingHours, e.TotalShows, e.IncidentCount);
}