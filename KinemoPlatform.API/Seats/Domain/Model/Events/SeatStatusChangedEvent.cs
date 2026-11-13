using KinemoPlatform.API.Seats.Domain.Model.ValueObjects;
using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Seats.Domain.Model.Events;

public record SeatStatusChangedEvent(int ShowId, int SeatNumber, ESeatStatus NewStatus) : IEvent;