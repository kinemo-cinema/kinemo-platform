namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;

public record ShowOccupancyResource(int Id, int ShowId, int TotalSeats, int OccupiedSeats, DateTimeOffset SynchronizedAt);