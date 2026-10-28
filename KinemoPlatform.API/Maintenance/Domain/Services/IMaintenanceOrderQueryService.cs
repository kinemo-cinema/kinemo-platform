using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;

namespace KinemoPlatform.API.Maintenance.Domain.Services;

public interface IMaintenanceOrderQueryService
{
    Task<IEnumerable<MaintenanceOrder>> HandleGetByEquipmentId(int equipmentId);
}