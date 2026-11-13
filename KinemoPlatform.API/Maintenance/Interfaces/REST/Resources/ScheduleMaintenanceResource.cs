using System.ComponentModel.DataAnnotations;
using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;

public record ScheduleMaintenanceResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidMaintenanceEquipmentId")]
    int EquipmentId,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidMaintenanceIncidentId")]
    int? IncidentId,
    [EnumDataType(typeof(EMaintenanceType), ErrorMessage = "InvalidMaintenanceType")]
    EMaintenanceType Type,
    DateTimeOffset ScheduledDate);
