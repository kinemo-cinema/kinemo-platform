namespace KinemoPlatform.API.Analytics.Domain.Model.Entities;

public class RoomPerformance
{
    public RoomPerformance()
    {
    }

    public RoomPerformance(int roomId, decimal operatingHours, int totalShows, int incidentCount) : this()
    {
        RoomId = roomId;
        OperatingHours = operatingHours;
        TotalShows = totalShows;
        IncidentCount = incidentCount;
    }

    public int Id { get; }
    public int DashboardId { get; private set; }
    public int RoomId { get; private set; }
    public decimal OperatingHours { get; private set; }
    public int TotalShows { get; private set; }
    public int IncidentCount { get; private set; }
}