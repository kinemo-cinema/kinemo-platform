using KinemoPlatform.API.Maintenance.Domain.Model.Commands;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
public static class RunEquipmentTestCommandFromResourceAssembler
{
    public static RunEquipmentTestCommand ToCommandFromResource(RunEquipmentTestResource r, int equipmentId) => 
        new(equipmentId, r.Channel);
}