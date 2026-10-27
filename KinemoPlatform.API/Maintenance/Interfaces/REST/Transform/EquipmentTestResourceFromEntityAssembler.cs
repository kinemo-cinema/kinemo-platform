using KinemoPlatform.API.Maintenance.Domain.Model.Entities;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
public static class EquipmentTestResourceFromEntityAssembler
{
    public static EquipmentTestResource ToResourceFromEntity(EquipmentTest e) => 
        new(e.Id, e.TestDate, e.Result, e.Observations, e.ChannelTests.Select(ChannelTestResourceFromEntityAssembler.ToResourceFromEntity));
}