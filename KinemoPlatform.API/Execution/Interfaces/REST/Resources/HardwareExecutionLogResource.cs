namespace KinemoPlatform.API.Execution.Interfaces.REST.Resources;

public record HardwareExecutionLogResource(
    int Id,
    DateTimeOffset OccurredAt,
    string Message,
    string Severity);