using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Entities;

public class EquipmentTest
{
    public EquipmentTest()
    {
        Observations = string.Empty;
        ChannelTests = new List<ChannelTest>();
    }

    public EquipmentTest(ETestResult result, string observations) : this()
    {
        Result = result;
        Observations = observations;
        TestDate = DateTimeOffset.UtcNow;
    }

    public int Id { get; }
    public int EquipmentId { get; private set; }
    public DateTimeOffset TestDate { get; private set; }
    public ETestResult Result { get; private set; }
    public string Observations { get; private set; }

    public ICollection<ChannelTest> ChannelTests { get; }

    public void AddChannelTest(EEffectChannel channel, int intensity, bool successful)
    {
        ChannelTests.Add(new ChannelTest(channel, intensity, successful));
    }
}