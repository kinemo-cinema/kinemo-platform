using KinemoPlatform.API.Maintenance.Domain.Model.Entities;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
public static class IncidentResourceFromEntityAssembler
{
    public static IncidentResource ToResourceFromEntity(Incident e) => 
        new(e.Id, e.Description, e.Severity, e.Status, e.ReportedAt, e.ResolvedAt);
}