using KinemoPlatform.API.Execution.Domain.Model.Entities;
using KinemoPlatform.API.Execution.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Transform;

public static class HardwareExecutionLogResourceFromEntityAssembler
{
    public static HardwareExecutionLogResource ToResourceFromEntity(HardwareExecutionLog entity)
    {
        return new HardwareExecutionLogResource(entity.Id, entity.OccurredAt, entity.Message, entity.Severity);
    }
}