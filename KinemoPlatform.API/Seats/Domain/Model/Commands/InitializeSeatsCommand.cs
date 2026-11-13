namespace KinemoPlatform.API.Seats.Domain.Model.Commands;

public record InitializeSeatsCommand(int ShowId, int TotalSeats);