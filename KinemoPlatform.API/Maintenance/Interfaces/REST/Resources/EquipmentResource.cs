using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
public record EquipmentResource(
    int Id, 
    int RoomId, 
    string Name, 
    EEquipmentType EquipmentType, 
    EEquipmentStatus Status, 
    int UsefulLifeHours, 
    int AccumulatedHours,
    IEnumerable<IncidentResource> Incidents,
    IEnumerable<EquipmentTestResource> Tests);