using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Entities;
using KinemoPlatform.API.Maintenance.Domain.Model.Queries;
using KinemoPlatform.API.Maintenance.Domain.Repositories;
using KinemoPlatform.API.Maintenance.Domain.Services;

namespace KinemoPlatform.API.Maintenance.Application.Internal.QueryServices;

public class EquipmentQueryService(IEquipmentRepository equipmentRepository) : IEquipmentQueryService
{
    public async Task<Equipment?> Handle(GetEquipmentByIdQuery query)
    {
        return await equipmentRepository.FindByIdWithDetailsAsync(query.EquipmentId);
    }

    public async Task<IEnumerable<Equipment>> Handle(GetAllEquipmentQuery query)
    {
        return await equipmentRepository.ListAsync();
    }

    public async Task<IEnumerable<Incident>> Handle(GetIncidentsByEquipmentIdQuery query)
    {
        var equipment = await equipmentRepository.FindByIdWithDetailsAsync(query.EquipmentId);
        return equipment?.Incidents ?? new List<Incident>();
    }
}