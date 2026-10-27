namespace KinemoPlatform.API.Execution.Domain.Model.Commands;

public record TriggerEmergencyStopCommand(int ExecutionId, string Reason);