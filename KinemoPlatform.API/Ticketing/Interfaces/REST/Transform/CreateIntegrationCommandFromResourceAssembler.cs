using KinemoPlatform.API.Ticketing.Domain.Model.Commands;
using KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;

namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Transform;

public static class CreateIntegrationCommandFromResourceAssembler
{
    public static CreateIntegrationCommand ToCommandFromResource(CreateIntegrationResource resource)
    {
        return new CreateIntegrationCommand(resource.CinemaId, resource.ProviderName);
    }
}