using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Seats.Domain.Model.Events;

public record SeatMarkedOutOfServiceEvent(int ShowId, int SeatNumber, string Reason) : IEvent;