namespace KinemoPlatform.API.Analytics.Domain.Model.Entities;

public class IncidentSummary
{
    public IncidentSummary()
    {
    }

    public IncidentSummary(int totalIncidents, int lowCount, int mediumCount, int highCount, int criticalCount) : this()
    {
        TotalIncidents = totalIncidents;
        LowCount = lowCount;
        MediumCount = mediumCount;
        HighCount = highCount;
        CriticalCount = criticalCount;
    }

    public int Id { get; }
    public int DashboardId { get; private set; }
    public int TotalIncidents { get; private set; }
    public int LowCount { get; private set; }
    public int MediumCount { get; private set; }
    public int HighCount { get; private set; }
    public int CriticalCount { get; private set; }
}