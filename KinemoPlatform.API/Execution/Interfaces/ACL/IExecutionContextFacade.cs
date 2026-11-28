namespace KinemoPlatform.API.Execution.Interfaces.ACL;

/// <summary>An emergency stop that happened during a show's execution.</summary>
public record EmergencyOccurrence(int ShowId, DateTimeOffset TriggeredAt);

/// <summary>Facade that exposes Execution data to other bounded contexts.</summary>
public interface IExecutionContextFacade
{
    /// <summary>Emergencies triggered in [from, to).</summary>
    Task<IReadOnlyList<EmergencyOccurrence>> FetchEmergencies(
        DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default);
}