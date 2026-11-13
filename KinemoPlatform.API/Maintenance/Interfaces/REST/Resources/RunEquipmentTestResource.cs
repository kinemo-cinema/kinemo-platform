using System.ComponentModel.DataAnnotations;
using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;

public record RunEquipmentTestResource(
    [EnumDataType(typeof(EEffectChannel), ErrorMessage = "InvalidEffectChannel")]
    EEffectChannel Channel);
