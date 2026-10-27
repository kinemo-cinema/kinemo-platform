using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Ticketing.Domain.Model.Events;

/// <summary>
///     Domain event published when a synchronization attempt fails.
/// </summary>
/// <param name="IntegrationId">The ID of the integration that failed.</param>
/// <param name="ShowId">The ID of the show being synced.</param>
/// <param name="ErrorDetails">The reason for the failure.</param>
public record SyncFailedEvent(int IntegrationId, int ShowId, string ErrorDetails) : IEvent;