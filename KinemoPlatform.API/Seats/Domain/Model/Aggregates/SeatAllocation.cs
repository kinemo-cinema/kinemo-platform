using KinemoPlatform.API.Seats.Domain.Model.Commands;
using KinemoPlatform.API.Seats.Domain.Model.Entities;
using KinemoPlatform.API.Seats.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Seats.Domain.Model.Aggregates;

/// <summary>
///     Seat Allocation aggregate root.
/// </summary>
/// <remarks>
///     Manages the seat map for a specific show, including statuses 
///     and manual interventions.
/// </remarks>
public partial class SeatAllocation
{
    public SeatAllocation()
    {
        ShowSeats = new List<ShowSeat>();
        Interventions = new List<ManualIntervention>();
    }

    public SeatAllocation(InitializeSeatsCommand command) : this()
    {
        ShowId = command.ShowId;
        
        // Generate the seats automatically based on the room capacity
        for (int i = 1; i <= command.TotalSeats; i++)
        {
            ShowSeats.Add(new ShowSeat(i));
        }
    }

    public int Id { get; }
    public int ShowId { get; private set; }

    public ICollection<ShowSeat> ShowSeats { get; }
    public ICollection<ManualIntervention> Interventions { get; }

    /// <summary>
    ///     Changes the status of a specific seat.
    /// </summary>
    public void AssignSeat(int seatNumber, ESeatStatus newStatus)
    {
        var seat = ShowSeats.FirstOrDefault(s => s.SeatNumber == seatNumber);
        if (seat is null) throw new InvalidOperationException($"Seat {seatNumber} not found in allocation.");

        switch (newStatus)
        {
            case ESeatStatus.Sold:
                seat.MarkAsSold();
                break;
            case ESeatStatus.Enabled:
                seat.Enable();
                break;
            case ESeatStatus.Disabled:
                seat.Disable();
                break;
            case ESeatStatus.OutOfService:
                seat.MarkOutOfService();
                break;
            default:
                throw new InvalidOperationException($"Status {newStatus} is not supported for manual assignment.");
        }
    }

    /// <summary>
    ///     Marks a seat as out of service and logs the intervention.
    /// </summary>
    public void MarkSeatOutOfService(int seatNumber, string reason, int operatorId = 1)
    {
        var seat = ShowSeats.FirstOrDefault(s => s.SeatNumber == seatNumber);
        if (seat is null) throw new InvalidOperationException($"Seat {seatNumber} not found in allocation.");

        seat.MarkOutOfService();
        Interventions.Add(new ManualIntervention(seatNumber, operatorId, reason));
    }
}