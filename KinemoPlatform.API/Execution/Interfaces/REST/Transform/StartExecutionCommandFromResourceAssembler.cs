using KinemoPlatform.API.Execution.Domain.Model.Commands;
using KinemoPlatform.API.Execution.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Transform;

public static class StartExecutionCommandFromResourceAssembler
{
    public static StartExecutionCommand ToCommandFromResource(StartExecutionResource resource)
    {
        return new StartExecutionCommand(resource.ShowId, resource.SensoryContentId);
    }
}