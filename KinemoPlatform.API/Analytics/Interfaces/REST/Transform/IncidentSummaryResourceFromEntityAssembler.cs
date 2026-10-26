using KinemoPlatform.API.Analytics.Domain.Model.Entities;
using KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Analytics.Interfaces.REST.Transform;
public static class IncidentSummaryResourceFromEntityAssembler
{
    public static IncidentSummaryResource ToResourceFromEntity(IncidentSummary e) => 
        new(e.Id, e.TotalIncidents, e.LowCount, e.MediumCount, e.HighCount, e.CriticalCount);
}