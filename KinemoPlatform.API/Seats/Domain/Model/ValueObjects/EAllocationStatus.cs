namespace KinemoPlatform.API.Seats.Domain.Model.ValueObjects;

/// <summary>Lifecycle of a seat allocation. "Ready" will be added when Execution needs it.</summary>
public enum EAllocationStatus
{
    Pending,
    Synchronized,
    Closed
}