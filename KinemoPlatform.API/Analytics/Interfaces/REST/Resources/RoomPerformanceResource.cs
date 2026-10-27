namespace KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
public record RoomPerformanceResource(int Id, int RoomId, decimal OperatingHours, int TotalShows, int IncidentCount);