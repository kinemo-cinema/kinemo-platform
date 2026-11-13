using KinemoPlatform.API.Maintenance.Domain.Model.Commands;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
public static class RegisterEquipmentCommandFromResourceAssembler
{
    public static RegisterEquipmentCommand ToCommandFromResource(RegisterEquipmentResource r) => 
        new(r.RoomId, r.Name, r.EquipmentType, r.UsefulLifeHours);
}