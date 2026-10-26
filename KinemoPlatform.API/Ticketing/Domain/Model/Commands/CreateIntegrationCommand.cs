namespace KinemoPlatform.API.Ticketing.Domain.Model.Commands;

public record CreateIntegrationCommand(int CinemaId, string ProviderName);