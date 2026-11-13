using KinemoPlatform.API.Seats.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Seats.Interfaces.REST.Resources;

public record ShowSeatResource(int Id, int SeatNumber, bool Sold, bool Enabled, ESeatStatus Status);