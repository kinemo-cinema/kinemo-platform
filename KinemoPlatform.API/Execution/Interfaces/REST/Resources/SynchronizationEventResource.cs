namespace KinemoPlatform.API.Execution.Interfaces.REST.Resources;

public record SynchronizationEventResource(
    int Id,
    long ExpectedTimeCode,
    long ActualTimeCode,
    long DriftMilliseconds,
    DateTimeOffset OccurredAt,
    string EventType);