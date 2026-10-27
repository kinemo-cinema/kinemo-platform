namespace KinemoPlatform.API.Ticketing.Domain.Model.Entities;

/// <summary>
///     Represents the seat occupancy for a specific show.
/// </summary>
public class ShowOccupancy
{
    public ShowOccupancy()
    {
    }

    public ShowOccupancy(int showId, int totalSeats, int occupiedSeats) : this()
    {
        ShowId = showId;
        TotalSeats = totalSeats;
        OccupiedSeats = occupiedSeats;
        SynchronizedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; }
    public int IntegrationId { get; private set; }
    public int ShowId { get; private set; }
    public int TotalSeats { get; private set; }
    public int OccupiedSeats { get; private set; }
    public DateTimeOffset SynchronizedAt { get; private set; }

    /// <summary>
    ///     Updates the occupancy counts from the latest sync.
    /// </summary>
    public void UpdateOccupancy(int totalSeats, int occupiedSeats)
    {
        TotalSeats = totalSeats;
        OccupiedSeats = occupiedSeats;
        SynchronizedAt = DateTimeOffset.UtcNow;
    }
}