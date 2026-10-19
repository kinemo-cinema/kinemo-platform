using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Scheduling.Domain.Model.Events;

/// <summary>
///     Domain event published when a new show is scheduled (US07).
/// </summary>
/// <param name="ShowId">The unique identifier of the scheduled show.</param>
/// <param name="MovieId">The movie being shown.</param>
/// <param name="RoomId">The room where the show will take place.</param>
/// <param name="StartDateTime">The start time of the show.</param>
public record ShowScheduledEvent(int ShowId, int MovieId, int RoomId, DateTimeOffset StartDateTime) : IEvent;