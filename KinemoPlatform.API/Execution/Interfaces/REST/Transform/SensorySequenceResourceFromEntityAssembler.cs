using KinemoPlatform.API.Execution.Domain.Model.Entities;
using KinemoPlatform.API.Execution.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Transform;

public static class SensorySequenceResourceFromEntityAssembler
{
    public static SensorySequenceResource ToResourceFromEntity(SensorySequence entity)
    {
        return new SensorySequenceResource(entity.Id, entity.SensoryTrackId, entity.CurrentTimeCode, entity.ActualTimeCode, entity.Status);
    }
}