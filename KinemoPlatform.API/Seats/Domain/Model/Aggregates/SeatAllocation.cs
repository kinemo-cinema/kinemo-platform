using KinemoPlatform.API.Seats.Domain.Model.Entities;
using KinemoPlatform.API.Seats.Domain.Model.Events;
using KinemoPlatform.API.Seats.Domain.Model.ValueObjects;
using KinemoPlatform.API.Shared.Domain.Model.Aggregates.Internal;

namespace KinemoPlatform.API.Seats.Domain.Model.Aggregates;

public partial class SeatAllocation : AggregateRoot
{
    // TODO(IAM): replace with the authenticated operator once authentication exists.
    public const int DefaultOperatorId = 1;

    public SeatAllocation()
    {
        ShowSeats = new List<ShowSeat>();
        Interventions = new List<ManualIntervention>();
        Status = EAllocationStatus.Pending;
    }

    public SeatAllocation(int showId, int totalSeats) : this()
    {
        ShowId = showId;
        for (var i = 1; i <= totalSeats; i++)
        {
            ShowSeats.Add(new ShowSeat(i));
        }
    }

    public int Id { get; }
    public int ShowId { get; private set; }
    public EAllocationStatus Status { get; private set; }
    public ICollection<ShowSeat> ShowSeats { get; }
    public ICollection<ManualIntervention> Interventions { get; }

    public bool IsClosed => Status == EAllocationStatus.Closed;

    public bool HasSeat(int seatNumber)
    {
        return ShowSeats.Any(s => s.SeatNumber == seatNumber);
    }

    public static bool IsManuallyAssignable(ESeatStatus status)
    {
        return status is ESeatStatus.Sold or ESeatStatus.Enabled or ESeatStatus.Disabled or ESeatStatus.OutOfService;
    }

    public void AssignSeat(int seatNumber, ESeatStatus newStatus, string? reason = null,
        int operatorId = DefaultOperatorId)
    {
        EnsureOpen();
        var seat = FindSeat(seatNumber);
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

        Interventions.Add(new ManualIntervention(seatNumber, operatorId,
            string.IsNullOrWhiteSpace(reason) ? $"Status changed to {newStatus}" : reason));
        AddDomainEvent(new SeatStatusChangedEvent(ShowId, seatNumber, newStatus));
    }

    public void MarkSeatOutOfService(int seatNumber, string reason, int operatorId = DefaultOperatorId)
    {
        EnsureOpen();
        FindSeat(seatNumber).MarkOutOfService();
        Interventions.Add(new ManualIntervention(seatNumber, operatorId, reason));
        AddDomainEvent(new SeatMarkedOutOfServiceEvent(ShowId, seatNumber, reason));
    }

    /// <summary>Ticketing occupancy was synchronized for this show. Only moves forward from Pending.</summary>
    public void MarkSynchronized()
    {
        if (Status == EAllocationStatus.Pending) Status = EAllocationStatus.Synchronized;
    }

    /// <summary>The show is over (or cancelled): the allocation can no longer be edited.</summary>
    public void Close()
    {
        Status = EAllocationStatus.Closed;
    }

    private void EnsureOpen()
    {
        if (IsClosed) throw new InvalidOperationException("The seat allocation is closed.");
    }

    private ShowSeat FindSeat(int seatNumber)
    {
        return ShowSeats.FirstOrDefault(s => s.SeatNumber == seatNumber)
               ?? throw new InvalidOperationException($"Seat {seatNumber} not found in allocation.");
    }
}