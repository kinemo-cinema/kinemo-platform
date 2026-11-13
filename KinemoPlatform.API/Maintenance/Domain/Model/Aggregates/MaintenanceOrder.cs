using KinemoPlatform.API.Maintenance.Domain.Model.Commands;
using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;

public partial class MaintenanceOrder
{
    public MaintenanceOrder()
    {
    }

    public MaintenanceOrder(ScheduleMaintenanceCommand command) : this()
    {
        EquipmentId = command.EquipmentId;
        IncidentId = command.IncidentId;
        MaintenanceType = command.Type;
        ScheduledDate = command.ScheduledDate;
        Status = EMaintenanceStatus.Scheduled;
    }

    public int Id { get; }
    public int EquipmentId { get; private set; }
    public int? IncidentId { get; private set; }
    public EMaintenanceType MaintenanceType { get; private set; }
    public EMaintenanceStatus Status { get; private set; }
    public DateTimeOffset ScheduledDate { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public string? CompletionNotes { get; private set; }

    public void Complete(string notes)
    {
        Status = EMaintenanceStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;
        CompletionNotes = notes;
    }

    public void Cancel()
    {
        Status = EMaintenanceStatus.Cancelled;
    }
}