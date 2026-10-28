using KinemoPlatform.API.Execution.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Resources;

public record EmergencyEventResource(
    int Id,
    string Reason,
    DateTimeOffset TriggeredAt,
    DateTimeOffset? ResolvedAt,
    EEmergencyStatus Status);