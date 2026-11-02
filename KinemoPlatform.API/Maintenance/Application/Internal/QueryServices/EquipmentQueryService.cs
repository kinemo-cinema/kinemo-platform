using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Entities;
using KinemoPlatform.API.Maintenance.Domain.Model.Queries;
using KinemoPlatform.API.Maintenance.Domain.Repositories;
using KinemoPlatform.API.Maintenance.Application.Services;

namespace KinemoPlatform.API.Maintenance.Application.Internal.QueryServices;

public class EquipmentQueryService(IEquipmentRepository equipmentRepository) : IEquipmentQueryService
{
    public async Task<Equipment?> Handle(GetEquipmentByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await equipmentRepository.FindByIdWithDetailsAsync(query.EquipmentId, cancellationToken);
    }

    public async Task<IEnumerable<Equipment>> Handle(GetAllEquipmentQuery query, CancellationToken cancellationToken = default)
    {
        return await equipmentRepository.ListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Incident>> Handle(GetIncidentsByEquipmentIdQuery query, CancellationToken cancellationToken = default)
    {
        var equipment = await equipmentRepository.FindByIdWithDetailsAsync(query.EquipmentId, cancellationToken);
        return equipment?.Incidents ?? new List<Incident>();
    }
}