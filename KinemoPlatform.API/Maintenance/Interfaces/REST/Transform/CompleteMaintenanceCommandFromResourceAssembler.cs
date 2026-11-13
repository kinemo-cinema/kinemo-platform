using KinemoPlatform.API.Maintenance.Domain.Model.Commands;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
public static class CompleteMaintenanceCommandFromResourceAssembler
{
    public static CompleteMaintenanceCommand ToCommandFromResource(CompleteMaintenanceResource r, int orderId) => 
        new(orderId, r.Notes);
}