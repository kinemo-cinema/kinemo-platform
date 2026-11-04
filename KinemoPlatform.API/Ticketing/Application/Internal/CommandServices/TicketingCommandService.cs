using Cortex.Mediator;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;
using KinemoPlatform.API.Ticketing.Application.Errors;
using KinemoPlatform.API.Ticketing.Application.Services;
using KinemoPlatform.API.Ticketing.Domain.ExternalServices;
using KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;
using KinemoPlatform.API.Ticketing.Domain.Model.Commands;
using KinemoPlatform.API.Ticketing.Domain.Model.Events;
using KinemoPlatform.API.Ticketing.Domain.Repositories;

namespace KinemoPlatform.API.Ticketing.Application.Internal.CommandServices;

/// <summary>
///     Implementation of the ticketing command service.
/// </summary>
/// <remarks>
///     Expected failures (integration not found) are returned as <see cref="ETicketingError" /> inside a
///     <see cref="Result{TValue,TError}" />. A failure of the external provider is recorded as a failed sync attempt
///     and returned as data. Other unexpected failures (e.g. database errors) are not caught here; they surface as
///     exceptions and are turned into a ProblemDetails 500 response globally.
/// </remarks>
public class TicketingCommandService(
    ITicketingRepository ticketingRepository,
    IExternalTicketingClient externalClient,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : ITicketingCommandService
{
    // Matches the maximum length of the SyncLog.ErrorDetails column.
    private const int MaxErrorDetailsLength = 1000;

    private const string NoDataMessage = "External system returned no data.";

    public async Task<Result<TicketingIntegration, ETicketingError>> Handle(CreateIntegrationCommand command,
        CancellationToken cancellationToken = default)
    {
        var integration = new TicketingIntegration(command);
        await ticketingRepository.AddAsync(integration, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Ok(integration);
    }

    public async Task<Result<TicketingIntegration, ETicketingError>> Handle(SyncOccupancyCommand command,
        CancellationToken cancellationToken = default)
    {
        var integration = await ticketingRepository.FindByIdWithDetailsAsync(command.IntegrationId, cancellationToken);
        if (integration is null) return Fail(ETicketingError.IntegrationNotFound);

        // Only the call to the external provider decides whether the sync failed.
        var (externalData, failureReason) =
            await FetchOccupancyAsync(integration.ProviderName, command.ShowId, cancellationToken);

        if (externalData is null)
        {
            var reason = failureReason ?? NoDataMessage;

            integration.RegisterFailedSync(command.ShowId, reason);
            ticketingRepository.Update(integration);
            await unitOfWork.CompleteAsync(cancellationToken);

            await domainEventPublisher.PublishAsync(
                new SyncFailedEvent(integration.Id, command.ShowId, reason), cancellationToken);

            return Ok(integration);
        }

        integration.RegisterSuccessfulSync(command.ShowId, externalData.TotalSeats, externalData.OccupiedSeats);
        ticketingRepository.Update(integration);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new OccupancySynchronizedEvent(integration.Id, command.ShowId, externalData.OccupiedSeats),
            cancellationToken);

        return Ok(integration);
    }

    /// <summary>
    ///     Calls the external provider and converts its failures into a reason, without throwing.
    /// </summary>
    /// <remarks>
    ///     Cancellation requested by the caller is not a provider failure, so it is not caught here. A provider
    ///     timeout (which also surfaces as an OperationCanceledException) is still recorded, because the caller's
    ///     token is not cancelled in that case.
    /// </remarks>
    private async Task<(ExternalOccupancyData? Data, string? FailureReason)> FetchOccupancyAsync(
        string providerName, int showId, CancellationToken cancellationToken)
    {
        try
        {
            var data = await externalClient.GetOccupancyAsync(providerName, showId, cancellationToken);
            return data is null ? (null, NoDataMessage) : (data, null);
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            return (null, Truncate(ex.Message));
        }
    }

    private static string Truncate(string text) =>
        text.Length <= MaxErrorDetailsLength ? text : text[..MaxErrorDetailsLength];

    private static Result<TicketingIntegration, ETicketingError> Ok(TicketingIntegration integration) =>
        new Result<TicketingIntegration, ETicketingError>.Success(integration);

    private static Result<TicketingIntegration, ETicketingError> Fail(ETicketingError error) =>
        new Result<TicketingIntegration, ETicketingError>.Failure(error);
}