namespace KinemoPlatform.API.Seats.Interfaces.REST.Resources;

public record SeatAllocationResource(
    int Id,
    int ShowId,
    IEnumerable<ShowSeatResource> ShowSeats);