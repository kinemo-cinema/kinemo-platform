using KinemoPlatform.API.Rooms.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Rooms.Interfaces.REST.Resources;

public record RoomResource(
    int Id,
    string Name,
    int Capacity,
    ERoomStatus Status,
    IEnumerable<RoomInspectionResource> Inspections,
    IEnumerable<RoomBlockResource> Blocks);