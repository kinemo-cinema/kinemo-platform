using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Repositories;
using KinemoPlatform.API.Maintenance.Domain.Services;

namespace KinemoPlatform.API.Maintenance.Application.Internal.QueryServices;

public class MaintenanceOrderQueryService(IMaintenanceOrderRepository maintenanceOrderRepository) : IMaintenanceOrderQueryService
{
    public async Task<IEnumerable<MaintenanceOrder>> HandleGetByEquipmentId(int equipmentId)
    {
        return await maintenanceOrderRepository.FindByEquipmentIdAsync(equipmentId);
    }
}