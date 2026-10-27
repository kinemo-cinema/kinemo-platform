using KinemoPlatform.API.Rooms.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

public record RoomInspectionResource(int Id, DateTimeOffset InspectedAt, string Observations, EInspectionResult Result);