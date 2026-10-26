using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Ticketing.Domain.Model.Events;

/// <summary>
///     Domain event published when a show's occupancy is successfully synced.
/// </summary>
/// <param name="IntegrationId">The ID of the integration used.</param>
/// <param name="ShowId">The ID of the show synced.</param>
/// <param name="OccupiedSeats">The updated number of occupied seats.</param>
public record OccupancySynchronizedEvent(int IntegrationId, int ShowId, int OccupiedSeats) : IEvent;