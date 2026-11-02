using System.ComponentModel.DataAnnotations;
namespace KinemoPlatform.API.Seats.Interfaces.REST.Resources;

public record MarkSeatOutOfServiceResource(
    [Required(ErrorMessage = "RequiredOutOfServiceReason")]
    [StringLength(500, ErrorMessage = "InvalidOutOfServiceReasonLength")]
    string Reason);
