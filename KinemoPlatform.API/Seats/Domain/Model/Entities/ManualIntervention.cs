namespace KinemoPlatform.API.Seats.Domain.Model.Entities;

/// <summary>
///     Represents a manual action taken by an operator on a seat.
/// </summary>
public class ManualIntervention
{
    public ManualIntervention()
    {
        Reason = string.Empty;
    }

    public ManualIntervention(int seatNumber, int operatorId, string reason) : this()
    {
        SeatNumber = seatNumber;
        OperatorId = operatorId;
        Reason = reason;
        InterventionDate = DateTimeOffset.UtcNow;
    }

    public int Id { get; }
    public int SeatAllocationId { get; private set; }
    public int SeatNumber { get; private set; }
    public int OperatorId { get; private set; }
    public string Reason { get; private set; }
    public DateTimeOffset InterventionDate { get; private set; }
}