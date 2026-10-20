using KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;
using KinemoPlatform.API.Ticketing.Domain.Model.Entities;

namespace KinemoPlatform.API.Ticketing.Domain.Repositories;

/// <summary>
///     Represents the ticketing integration repository.
/// </summary>
public interface ITicketingRepository : IBaseRepository<TicketingIntegration>
{
    /// <summary>
    ///     Retrieves an integration by its ID, eagerly loading sync logs and occupancies.
    /// </summary>
    Task<TicketingIntegration?> FindByIdWithDetailsAsync(int integrationId);

    /// <summary>
    ///     Retrieves the occupancy record for a specific show.
    /// </summary>
    Task<ShowOccupancy?> FindOccupancyByShowIdAsync(int showId);
}