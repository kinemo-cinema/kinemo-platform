using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Commands;

namespace KinemoPlatform.API.Maintenance.Application.Services;

public interface IMaintenanceOrderCommandService
{
    Task<MaintenanceOrder?> Handle(ScheduleMaintenanceCommand command);
    Task<MaintenanceOrder?> Handle(CompleteMaintenanceCommand command);
}