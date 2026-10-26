using KinemoPlatform.API.Ticketing.Domain.Model.Entities;
using KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Transform;

public static class SyncLogResourceFromEntityAssembler
{
    public static SyncLogResource ToResourceFromEntity(SyncLog entity)
    {
        return new SyncLogResource(entity.Id, entity.ShowId, entity.ExecutedAt, entity.Status, entity.ErrorDetails);
    }
}