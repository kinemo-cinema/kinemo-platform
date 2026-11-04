using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Ticketing.Application.Errors;
using KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;
using KinemoPlatform.API.Ticketing.Domain.Model.Commands;

namespace KinemoPlatform.API.Ticketing.Application.Services;

/// <summary>
///     Represents the ticketing command service in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Every operation returns a success result with the affected <see cref="TicketingIntegration" />, or a failure
///     with an <see cref="ETicketingError" />.
/// </remarks>
public interface ITicketingCommandService
{
    /// <summary>
    ///     Handles the registration of a new external ticketing integration.
    /// </summary>
    Task<Result<TicketingIntegration, ETicketingError>> Handle(CreateIntegrationCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles the synchronization of a show's occupancy with the external provider.
    /// </summary>
    /// <remarks>
    ///     A failure of the external provider is not an error of the request: it is recorded as a failed
    ///     <c>SyncLog</c> of the integration, a <c>SyncFailedEvent</c> is published and the result is a success.
    /// </remarks>
    Task<Result<TicketingIntegration, ETicketingError>> Handle(SyncOccupancyCommand command,
        CancellationToken cancellationToken = default);
}