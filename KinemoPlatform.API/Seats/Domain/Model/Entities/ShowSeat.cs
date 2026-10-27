using KinemoPlatform.API.Seats.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Seats.Domain.Model.Entities;

/// <summary>
///     Represents a specific seat within a show's allocation.
/// </summary>
public class ShowSeat
{
    public ShowSeat()
    {
    }

    public ShowSeat(int seatNumber) : this()
    {
        SeatNumber = seatNumber;
        Status = ESeatStatus.Available;
        Sold = false;
        Enabled = true;
    }

    public int Id { get; }
    public int SeatAllocationId { get; private set; }
    public int SeatNumber { get; private set; }
    public bool Sold { get; private set; }
    public bool Enabled { get; private set; }
    public ESeatStatus Status { get; private set; }

    public void MarkAsSold()
    {
        Status = ESeatStatus.Sold;
        Sold = true;
    }

    public void Disable()
    {
        Status = ESeatStatus.Disabled;
        Enabled = false;
    }

    public void Enable()
    {
        Status = ESeatStatus.Enabled;
        Enabled = true;
    }

    public void MarkOutOfService()
    {
        Status = ESeatStatus.OutOfService;
        Enabled = false;
    }
}