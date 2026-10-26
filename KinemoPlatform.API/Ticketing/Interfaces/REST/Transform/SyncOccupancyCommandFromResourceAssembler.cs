using KinemoPlatform.API.Ticketing.Domain.Model.Commands;
using KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Transform;

public static class SyncOccupancyCommandFromResourceAssembler
{
    public static SyncOccupancyCommand ToCommandFromResource(SyncOccupancyResource resource, int integrationId)
    {
        return new SyncOccupancyCommand(integrationId, resource.ShowId);
    }
}