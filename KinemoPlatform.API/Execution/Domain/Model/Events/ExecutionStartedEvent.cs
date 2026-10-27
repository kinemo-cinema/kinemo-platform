using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Execution.Domain.Model.Events;

public record ExecutionStartedEvent(int ExecutionId, int ShowId) : IEvent;