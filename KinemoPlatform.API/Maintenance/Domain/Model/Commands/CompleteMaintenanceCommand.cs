namespace KinemoPlatform.API.Maintenance.Domain.Model.Commands;

public record CompleteMaintenanceCommand(int MaintenanceOrderId, string Notes);