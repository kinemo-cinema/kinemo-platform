using System.ComponentModel.DataAnnotations;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;

public record CompleteMaintenanceResource(
    [Required(ErrorMessage = "RequiredCompletionNotes")]
    [StringLength(1000, ErrorMessage = "InvalidCompletionNotesLength")]
    string Notes);
