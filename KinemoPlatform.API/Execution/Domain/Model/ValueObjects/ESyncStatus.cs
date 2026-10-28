namespace KinemoPlatform.API.Execution.Domain.Model.ValueObjects;

/// <summary>
///     Represents the synchronization status of a specific sensory sequence.
/// </summary>
public enum ESyncStatus
{
    Synchronized,
    Drifting,
    Lost
}