using KinemoPlatform.API.Maintenance.Domain.Model.Entities;
using KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Transform;
public static class ChannelTestResourceFromEntityAssembler
{
    public static ChannelTestResource ToResourceFromEntity(ChannelTest e) => 
        new(e.Id, e.EffectChannel, e.Intensity, e.Successful);
}