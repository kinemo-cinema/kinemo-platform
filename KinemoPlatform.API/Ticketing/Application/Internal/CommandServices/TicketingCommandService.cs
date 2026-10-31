using Cortex.Mediator;
using KinemoPlatform.API.Shared.Domain.Repositories;
using KinemoPlatform.API.Ticketing.Application.Services;
using KinemoPlatform.API.Ticketing.Domain.ExternalServices;
using KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;
using KinemoPlatform.API.Ticketing.Domain.Model.Commands;
using KinemoPlatform.API.Ticketing.Domain.Model.Events;
using KinemoPlatform.API.Ticketing.Domain.Repositories;

namespace KinemoPlatform.API.Ticketing.Application.Internal.CommandServices;

public class TicketingCommandService(
    ITicketingRepository ticketingRepository,
    IExternalTicketingClient externalClient,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : ITicketingCommandService
{
    public async Task<TicketingIntegration?> Handle(CreateIntegrationCommand command)
    {
        var integration = new TicketingIntegration(command);
        await ticketingRepository.AddAsync(integration);
        await unitOfWork.CompleteAsync();
        return integration;
    }

    public async Task<TicketingIntegration?> Handle(SyncOccupancyCommand command)
    {
        var integration = await ticketingRepository.FindByIdWithDetailsAsync(command.IntegrationId);
        if (integration is null) throw new InvalidOperationException("Integration not found.");

        try
        {
            // Call the external system
            var externalData = await externalClient.GetOccupancyAsync(integration.ProviderName, command.ShowId);
            
            if (externalData is null)
                throw new Exception("External system returned no data.");

            // Update the aggregate
            integration.RegisterSuccessfulSync(command.ShowId, externalData.TotalSeats, externalData.OccupiedSeats);
            ticketingRepository.Update(integration);
            await unitOfWork.CompleteAsync();

            // Publish success event
            await domainEventPublisher.PublishAsync(
                new OccupancySynchronizedEvent(integration.Id, command.ShowId, externalData.OccupiedSeats));
        }
        catch (Exception ex)
        {
            // Handle failure (log it, but don't crash the API)
            integration.RegisterFailedSync(command.ShowId, ex.Message);
            ticketingRepository.Update(integration);
            await unitOfWork.CompleteAsync();

            // Publish failure event
            await domainEventPublisher.PublishAsync(
                new SyncFailedEvent(integration.Id, command.ShowId, ex.Message));
        }

        return integration;
    }
}