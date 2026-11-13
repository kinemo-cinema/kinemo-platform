using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
public record ChannelTestResource(int Id, EEffectChannel EffectChannel, int Intensity, bool Successful);