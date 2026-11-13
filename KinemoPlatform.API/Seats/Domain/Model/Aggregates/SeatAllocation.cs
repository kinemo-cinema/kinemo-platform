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
    ///     Whether this allocation contains a seat with the given number.
    /// </summary>
    /// <param name="seatNumber">The seat number to look for.</param>
    /// <returns>True when the seat exists.</returns>
    public bool HasSeat(int seatNumber)
    {
        return ShowSeats.Any(s => s.SeatNumber == seatNumber);
    }

    /// <summary>
    ///     Whether a status can be assigned manually with <see cref="AssignSeat" />.
    /// </summary>
    /// <remarks>
    ///     Keep in sync with the cases handled by <see cref="AssignSeat" />. Callers should check this (and
    ///     <see cref="HasSeat" />) before assigning, so invalid input never has to be handled as an exception.
    /// </remarks>
    /// <param name="status">The status to check.</param>
    /// <returns>True when the status is supported for manual assignment.</returns>
    public static bool IsManuallyAssignable(ESeatStatus status)
    {
        return status is ESeatStatus.Sold or ESeatStatus.Enabled or ESeatStatus.Disabled or ESeatStatus.OutOfService;
    }

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