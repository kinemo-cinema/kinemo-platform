using Cortex.Mediator;
using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Commands;
using KinemoPlatform.API.Maintenance.Domain.Model.Events;
using KinemoPlatform.API.Maintenance.Domain.Repositories;
using KinemoPlatform.API.Maintenance.Domain.Services;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Maintenance.Application.Internal.CommandServices;

public class MaintenanceOrderCommandService(
    IMaintenanceOrderRepository maintenanceOrderRepository,
    IEquipmentRepository equipmentRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IMaintenanceOrderCommandService
{
    public async Task<MaintenanceOrder?> Handle(ScheduleMaintenanceCommand command)
    {
        // Verify equipment exists before scheduling
        var equipment = await equipmentRepository.FindByIdAsync(command.EquipmentId);
        if (equipment is null) throw new InvalidOperationException($"Equipment {command.EquipmentId} not found.");

        var order = new MaintenanceOrder(command);
        await maintenanceOrderRepository.AddAsync(order);
        await unitOfWork.CompleteAsync();

        return order;
    }

    public async Task<MaintenanceOrder?> Handle(CompleteMaintenanceCommand command)
    {
        var order = await maintenanceOrderRepository.FindByIdAsync(command.MaintenanceOrderId);
        if (order is null) throw new InvalidOperationException($"Maintenance Order {command.MaintenanceOrderId} not found.");

        order.Complete(command.Notes);
        maintenanceOrderRepository.Update(order);

        // If this was a corrective maintenance tied to an incident, resolve the incident
        if (order.IncidentId.HasValue)
        {
            var equipment = await equipmentRepository.FindByIdWithDetailsAsync(order.EquipmentId);
            var incident = equipment?.Incidents.FirstOrDefault(i => i.Id == order.IncidentId.Value);
            if (incident is not null)
            {
                incident.Resolve();
                equipmentRepository.Update(equipment!);
            }
        }

        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new MaintenanceCompletedEvent(order.Id, order.EquipmentId));

        return order;
    }
}