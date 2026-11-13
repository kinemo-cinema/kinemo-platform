using Cortex.Mediator;
using KinemoPlatform.API.Maintenance.Application.Errors;
using KinemoPlatform.API.Maintenance.Application.Services;
using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Commands;
using KinemoPlatform.API.Maintenance.Domain.Model.Events;
using KinemoPlatform.API.Maintenance.Domain.Repositories;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Maintenance.Application.Internal.CommandServices;

/// <summary>
///     Implementation of the maintenance order command service.
/// </summary>
/// <remarks>
///     Expected failures (equipment or order not found) are returned as <see cref="EMaintenanceOrderError" /> inside a
///     <see cref="Result{TValue,TError}" />. Unexpected failures are not caught here; they surface as exceptions and
///     are turned into a ProblemDetails 500 response globally.
/// </remarks>
public class MaintenanceOrderCommandService(
    IMaintenanceOrderRepository maintenanceOrderRepository,
    IEquipmentRepository equipmentRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IMaintenanceOrderCommandService
{
    public async Task<Result<MaintenanceOrder, EMaintenanceOrderError>> Handle(ScheduleMaintenanceCommand command,
        CancellationToken cancellationToken = default)
    {
        // Verify equipment exists before scheduling
        var equipment = await equipmentRepository.FindByIdAsync(command.EquipmentId, cancellationToken);
        if (equipment is null) return Fail(EMaintenanceOrderError.EquipmentNotFound);

        var order = new MaintenanceOrder(command);
        await maintenanceOrderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Ok(order);
    }

    public async Task<Result<MaintenanceOrder, EMaintenanceOrderError>> Handle(CompleteMaintenanceCommand command,
        CancellationToken cancellationToken = default)
    {
        var order = await maintenanceOrderRepository.FindByIdAsync(command.MaintenanceOrderId, cancellationToken);
        if (order is null) return Fail(EMaintenanceOrderError.OrderNotFound);

        order.Complete(command.Notes);
        maintenanceOrderRepository.Update(order);

        // If this was a corrective maintenance tied to an incident, resolve the incident
        if (order.IncidentId.HasValue)
        {
            var equipment = await equipmentRepository.FindByIdWithDetailsAsync(order.EquipmentId, cancellationToken);
            var incident = equipment?.Incidents.FirstOrDefault(i => i.Id == order.IncidentId.Value);
            if (incident is not null)
            {
                incident.Resolve();
                equipmentRepository.Update(equipment!);
            }
        }

        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new MaintenanceCompletedEvent(order.Id, order.EquipmentId), cancellationToken);

        return Ok(order);
    }

    private static Result<MaintenanceOrder, EMaintenanceOrderError> Ok(MaintenanceOrder order) =>
        new Result<MaintenanceOrder, EMaintenanceOrderError>.Success(order);

    private static Result<MaintenanceOrder, EMaintenanceOrderError> Fail(EMaintenanceOrderError error) =>
        new Result<MaintenanceOrder, EMaintenanceOrderError>.Failure(error);
}
