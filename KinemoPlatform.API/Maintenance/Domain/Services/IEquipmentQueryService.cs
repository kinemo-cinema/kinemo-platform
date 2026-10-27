using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Entities;
using KinemoPlatform.API.Maintenance.Domain.Model.Queries;

namespace KinemoPlatform.API.Maintenance.Domain.Services;

public interface IEquipmentQueryService
{
    Task<Equipment?> Handle(GetEquipmentByIdQuery query);
    Task<IEnumerable<Equipment>> Handle(GetAllEquipmentQuery query);
    Task<IEnumerable<Incident>> Handle(GetIncidentsByEquipmentIdQuery query);
}