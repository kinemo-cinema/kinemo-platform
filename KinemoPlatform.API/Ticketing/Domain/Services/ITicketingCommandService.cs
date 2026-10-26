using KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;
using KinemoPlatform.API.Ticketing.Domain.Model.Commands;

namespace KinemoPlatform.API.Ticketing.Domain.Services;

public interface ITicketingCommandService
{
    /// <summary>
    ///     Handles the creation of a new ticketing integration.
    /// </summary>
    Task<TicketingIntegration?> Handle(CreateIntegrationCommand command);

    /// <summary>
    ///     Handles the synchronization of a show's occupancy from the external system.
    /// </summary>
    Task<TicketingIntegration?> Handle(SyncOccupancyCommand command);
}