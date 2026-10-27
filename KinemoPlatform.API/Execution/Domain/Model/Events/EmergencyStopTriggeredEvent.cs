using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Execution.Domain.Model.Events;

public record EmergencyStopTriggeredEvent(int ExecutionId, string Reason) : IEvent;