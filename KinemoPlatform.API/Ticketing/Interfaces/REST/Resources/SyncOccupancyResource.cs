using System.ComponentModel.DataAnnotations;
namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;

public record SyncOccupancyResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidTicketingShowId")]
    int ShowId);