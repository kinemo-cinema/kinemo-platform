using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;

namespace KinemoPlatform.API.Maintenance.Application.Services;

public interface IMaintenanceOrderQueryService
{
    Task<IEnumerable<MaintenanceOrder>> HandleGetByEquipmentId(int equipmentId);
}