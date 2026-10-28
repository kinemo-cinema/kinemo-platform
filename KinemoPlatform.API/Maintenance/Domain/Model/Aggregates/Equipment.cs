using KinemoPlatform.API.Maintenance.Domain.Model.Commands;
using KinemoPlatform.API.Maintenance.Domain.Model.Entities;
using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;

public partial class Equipment
{
    public Equipment()
    {
        Name = string.Empty;
        Status = EEquipmentStatus.Operational;
        Incidents = new List<Incident>();
        Tests = new List<EquipmentTest>();
    }

    public Equipment(RegisterEquipmentCommand command) : this()
    {
        RoomId = command.RoomId;
        Name = command.Name;
        EquipmentType = command.EquipmentType;
        UsefulLifeHours = command.UsefulLifeHours;
        InstallationDate = DateTimeOffset.UtcNow;
        AccumulatedHours = 0;
    }

    public int Id { get; }
    public int RoomId { get; private set; }
    public string Name { get; private set; }
    public EEquipmentType EquipmentType { get; private set; }
    public EEquipmentStatus Status { get; private set; }
    public DateTimeOffset InstallationDate { get; private set; }
    public int UsefulLifeHours { get; private set; }
    public int AccumulatedHours { get; private set; }

    public ICollection<Incident> Incidents { get; }
    public ICollection<EquipmentTest> Tests { get; }

    public Incident ReportIncident(string description, EIncidentSeverity severity)
    {
        var incident = new Incident(description, severity);
        Incidents.Add(incident);
        
        if (severity == EIncidentSeverity.Critical)
            Status = EEquipmentStatus.Faulty;

        return incident;
    }

    public EquipmentTest RunTest(EEffectChannel channel)
    {
        Status = EEquipmentStatus.Testing;
        
        // Mock logic: If accumulated hours > useful life, test fails
        bool success = AccumulatedHours < UsefulLifeHours;
        
        var test = new EquipmentTest(success ? ETestResult.Passed : ETestResult.Failed, "Automated diagnostic");
        test.AddChannelTest(channel, 100, success);
        
        Tests.Add(test);
        Status = success ? EEquipmentStatus.Operational : EEquipmentStatus.Faulty;

        return test;
    }
}