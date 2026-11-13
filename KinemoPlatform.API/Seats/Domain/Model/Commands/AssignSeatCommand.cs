using KinemoPlatform.API.Seats.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Seats.Domain.Model.Commands;

public record AssignSeatCommand(int ShowId, int SeatNumber, ESeatStatus NewStatus);