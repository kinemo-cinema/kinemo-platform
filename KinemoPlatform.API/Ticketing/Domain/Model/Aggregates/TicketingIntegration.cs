using KinemoPlatform.API.Ticketing.Domain.Model.Commands;
using KinemoPlatform.API.Ticketing.Domain.Model.Entities;
using KinemoPlatform.API.Ticketing.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;

/// <summary>
///     Ticketing Integration aggregate root.
/// </summary>
/// <remarks>
///     Manages the connection to an external ticketing provider,
///     logging sync attempts and maintaining current show occupancies.
/// </remarks>
public partial class TicketingIntegration
{
    public TicketingIntegration()
    {
        ProviderName = string.Empty;
        Status = EIntegrationStatus.Disconnected;
        SyncLogs = new List<SyncLog>();
        Occupancies = new List<ShowOccupancy>();
    }

    public TicketingIntegration(CreateIntegrationCommand command) : this()
    {
        CinemaId = command.CinemaId;
        ProviderName = command.ProviderName;
        Status = EIntegrationStatus.Connected; // Assume connected on registration
        LastConnectionAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; }
    public int CinemaId { get; private set; }
    public string ProviderName { get; private set; }
    public EIntegrationStatus Status { get; private set; }
    public DateTimeOffset? LastConnectionAt { get; private set; }

    public ICollection<SyncLog> SyncLogs { get; }
    public ICollection<ShowOccupancy> Occupancies { get; }

    /// <summary>
    ///     Records a successful sync attempt and updates (or creates) the show's occupancy.
    /// </summary>
    public void RegisterSuccessfulSync(int showId, int totalSeats, int occupiedSeats)
    {
        SyncLogs.Add(new SyncLog(showId, ESyncStatus.Success));
        LastConnectionAt = DateTimeOffset.UtcNow;
        Status = EIntegrationStatus.Connected;

        var occupancy = Occupancies.FirstOrDefault(o => o.ShowId == showId);
        if (occupancy is null)
        {
            Occupancies.Add(new ShowOccupancy(showId, totalSeats, occupiedSeats));
        }
        else
        {
            occupancy.UpdateOccupancy(totalSeats, occupiedSeats);
        }
    }

    /// <summary>
    ///     Records a failed sync attempt.
    /// </summary>
    public void RegisterFailedSync(int showId, string errorDetails)
    {
        SyncLogs.Add(new SyncLog(showId, ESyncStatus.Failed, errorDetails));
        LastConnectionAt = DateTimeOffset.UtcNow;
        // Optionally change status to Error if there are too many failures, but keep Connected for now
    }
}