using System.ComponentModel.DataAnnotations;
using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;

public record RegisterIncidentResource(
    [Required(ErrorMessage = "RequiredIncidentDescription")]
    [StringLength(1000, ErrorMessage = "InvalidIncidentDescriptionLength")]
    string Description,
    [EnumDataType(typeof(EIncidentSeverity), ErrorMessage = "InvalidIncidentSeverity")]
    EIncidentSeverity Severity);
