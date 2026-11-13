using System.ComponentModel.DataAnnotations;

namespace KinemoPlatform.API.Execution.Interfaces.REST.Resources;

public record TriggerEmergencyStopResource(
    [Required(ErrorMessage = "RequiredEmergencyStopReason")]
    [StringLength(500, ErrorMessage = "InvalidEmergencyStopReasonLength")] string Reason);