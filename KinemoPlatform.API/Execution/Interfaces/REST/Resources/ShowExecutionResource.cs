using KinemoPlatform.API.Execution.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Resources;

public record ShowExecutionResource(
    int Id,
    int ShowId,
    int SensoryContentId,
    EExecutionStatus Status,
    DateTimeOffset StartedAt,
    DateTimeOffset? FinishedAt,
    IEnumerable<SensorySequenceResource> Sequences,
    IEnumerable<SynchronizationEventResource> SyncEvents,
    IEnumerable<HardwareExecutionLogResource> HardwareLogs,
    IEnumerable<EmergencyEventResource> EmergencyEvents);