namespace KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
public record IncidentSummaryResource(int Id, int TotalIncidents, int LowCount, int MediumCount, int HighCount, int CriticalCount);