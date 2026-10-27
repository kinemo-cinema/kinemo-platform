using KinemoPlatform.API.Execution.Domain.Model.Aggregates;
using KinemoPlatform.API.Execution.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Transform;

public static class ShowExecutionResourceFromEntityAssembler
{
    public static ShowExecutionResource ToResourceFromEntity(ShowExecution entity)
    {
        return new ShowExecutionResource(
            entity.Id,
            entity.ShowId,
            entity.SensoryContentId,
            entity.Status,
            entity.StartedAt,
            entity.FinishedAt,
            entity.Sequences.Select(SensorySequenceResourceFromEntityAssembler.ToResourceFromEntity),
            entity.SyncEvents.Select(SynchronizationEventResourceFromEntityAssembler.ToResourceFromEntity),
            entity.HardwareLogs.Select(HardwareExecutionLogResourceFromEntityAssembler.ToResourceFromEntity),
            entity.EmergencyEvents.Select(EmergencyEventResourceFromEntityAssembler.ToResourceFromEntity));
    }
}