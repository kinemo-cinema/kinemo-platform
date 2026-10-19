using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Scheduling.Domain.Model.Events;

/// <summary>
///     Domain event published when a show is cancelled (US08).
/// </summary>
/// <param name="ShowId">The unique identifier of the cancelled show.</param>
public record ShowCancelledEvent(int ShowId) : IEvent;