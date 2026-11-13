using System.ComponentModel.DataAnnotations;
using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;

public record RegisterEquipmentResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidEquipmentRoomId")]
    int RoomId,
    [Required(ErrorMessage = "RequiredEquipmentName")]
    [StringLength(150, ErrorMessage = "InvalidEquipmentNameLength")]
    string Name,
    [EnumDataType(typeof(EEquipmentType), ErrorMessage = "InvalidEquipmentType")]
    EEquipmentType EquipmentType,
    [Range(1, 1000000, ErrorMessage = "InvalidUsefulLifeHours")]
    int UsefulLifeHours);
