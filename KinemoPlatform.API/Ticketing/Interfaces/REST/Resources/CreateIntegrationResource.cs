using System.ComponentModel.DataAnnotations;
namespace KinemoPlatform.API.Ticketing.Interfaces.REST.Resources;

public record CreateIntegrationResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidCinemaId")]
    int CinemaId,
    [Required(ErrorMessage = "RequiredProviderName")]
    [StringLength(150, ErrorMessage = "InvalidProviderNameLength")]
    string ProviderName);