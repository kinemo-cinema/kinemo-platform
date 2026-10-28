using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Entities;

public class ChannelTest
{
    public ChannelTest()
    {
    }

    public ChannelTest(EEffectChannel effectChannel, int intensity, bool successful) : this()
    {
        EffectChannel = effectChannel;
        Intensity = intensity;
        Successful = successful;
    }

    public int Id { get; }
    public int EquipmentTestId { get; private set; }
    public EEffectChannel EffectChannel { get; private set; }
    public int Intensity { get; private set; }
    public bool Successful { get; private set; }
}