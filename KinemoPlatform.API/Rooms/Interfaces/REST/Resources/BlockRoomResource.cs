using System.ComponentModel.DataAnnotations;
namespace KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

public record BlockRoomResource(
    [Required(ErrorMessage = "RequiredBlockReason")]
    [StringLength(500, ErrorMessage = "InvalidBlockReasonLength")]
    string Reason,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime);