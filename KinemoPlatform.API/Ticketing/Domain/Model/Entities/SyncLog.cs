using KinemoPlatform.API.Ticketing.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Ticketing.Domain.Model.Entities;

/// <summary>
///     Represents a single synchronization attempt with the external provider.
/// </summary>
public class SyncLog
{
    public SyncLog()
    {
        ErrorDetails = string.Empty;
    }

    public SyncLog(int showId, ESyncStatus status, string errorDetails = "") : this()
    {
        ShowId = showId;
        Status = status;
        ErrorDetails = errorDetails;
        ExecutedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; }
    public int IntegrationId { get; private set; }
    public int ShowId { get; private set; }
    public DateTimeOffset ExecutedAt { get; private set; }
    public ESyncStatus Status { get; private set; }
    public string ErrorDetails { get; private set; }
}