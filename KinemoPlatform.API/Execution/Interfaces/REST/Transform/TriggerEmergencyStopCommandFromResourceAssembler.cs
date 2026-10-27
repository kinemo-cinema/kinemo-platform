using KinemoPlatform.API.Execution.Domain.Model.Commands;
using KinemoPlatform.API.Execution.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Transform;

public static class TriggerEmergencyStopCommandFromResourceAssembler
{
    public static TriggerEmergencyStopCommand ToCommandFromResource(TriggerEmergencyStopResource resource, int executionId)
    {
        return new TriggerEmergencyStopCommand(executionId, resource.Reason);
    }
}