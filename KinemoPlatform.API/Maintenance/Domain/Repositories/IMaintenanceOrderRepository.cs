using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Maintenance.Domain.Repositories;

/// <summary>
///     Represents the maintenance order repository in the Kinemo Platform.
/// </summary>
public interface IMaintenanceOrderRepository : IBaseRepository<MaintenanceOrder>
{
    /// <summary>
    ///     Retrieves all maintenance orders for a specific piece of equipment.
    /// </summary>
    Task<IEnumerable<MaintenanceOrder>> FindByEquipmentIdAsync(int equipmentId);
}