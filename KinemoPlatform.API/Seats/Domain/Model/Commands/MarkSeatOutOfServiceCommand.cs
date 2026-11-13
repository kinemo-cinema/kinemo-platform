namespace KinemoPlatform.API.Seats.Domain.Model.Commands;

public record MarkSeatOutOfServiceCommand(int ShowId, int SeatNumber, string Reason);