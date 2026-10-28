using KinemoPlatform.API.Execution.Domain.Model.Entities;
using KinemoPlatform.API.Execution.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Transform;

public static class SynchronizationEventResourceFromEntityAssembler
{
    public static SynchronizationEventResource ToResourceFromEntity(SynchronizationEvent entity)
    {
        return new SynchronizationEventResource(entity.Id, entity.ExpectedTimeCode, entity.ActualTimeCode, entity.DriftMilliseconds, entity.OccurredAt, entity.EventType);
    }
}