using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Commands;

namespace KinemoPlatform.API.Maintenance.Domain.Services;

public interface IEquipmentCommandService
{
    Task<Equipment?> Handle(RegisterEquipmentCommand command);
    Task<Equipment?> Handle(RegisterIncidentCommand command);
    Task<Equipment?> Handle(RunEquipmentTestCommand command);
}