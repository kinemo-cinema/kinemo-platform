using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Maintenance.Domain.Repositories;

/// <summary>
///     Represents the equipment repository in the Kinemo Platform.
/// </summary>
public interface IEquipmentRepository : IBaseRepository<Equipment>
{
    /// <summary>
    ///     Checks if an equipment with the given name already exists in a specific room.
    /// </summary>
    Task<bool> ExistsByNameAndRoomAsync(string name, int roomId);

    /// <summary>
    ///     Retrieves equipment by ID, eagerly loading incidents and tests.
    /// </summary>
    Task<Equipment?> FindByIdWithDetailsAsync(int equipmentId);
}