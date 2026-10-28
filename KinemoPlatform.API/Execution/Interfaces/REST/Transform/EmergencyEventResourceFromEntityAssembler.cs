using KinemoPlatform.API.Execution.Domain.Model.Entities;
using KinemoPlatform.API.Execution.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Transform;

public static class EmergencyEventResourceFromEntityAssembler
{
    public static EmergencyEventResource ToResourceFromEntity(EmergencyEvent entity)
    {
        return new EmergencyEventResource(entity.Id, entity.Reason, entity.TriggeredAt, entity.ResolvedAt, entity.Status);
    }
}