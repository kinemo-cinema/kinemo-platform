using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Entities;
using KinemoPlatform.API.Maintenance.Domain.Model.Queries;

namespace KinemoPlatform.API.Maintenance.Application.Services;

public interface IEquipmentQueryService
{
    Task<Equipment?> Handle(GetEquipmentByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Equipment>> Handle(GetAllEquipmentQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Incident>> Handle(GetIncidentsByEquipmentIdQuery query, CancellationToken cancellationToken = default);
}