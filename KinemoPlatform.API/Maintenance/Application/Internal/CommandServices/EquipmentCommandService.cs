using Cortex.Mediator;
using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Commands;
using KinemoPlatform.API.Maintenance.Domain.Model.Events;
using KinemoPlatform.API.Maintenance.Domain.Repositories;
using KinemoPlatform.API.Maintenance.Domain.Services;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Maintenance.Application.Internal.CommandServices;

public class EquipmentCommandService(
    IEquipmentRepository equipmentRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IEquipmentCommandService
{
    public async Task<Equipment?> Handle(RegisterEquipmentCommand command)
    {
        if (await equipmentRepository.ExistsByNameAndRoomAsync(command.Name, command.RoomId))
            throw new InvalidOperationException($"Equipment '{command.Name}' already exists in Room {command.RoomId}.");

        var equipment = new Equipment(command);
        await equipmentRepository.AddAsync(equipment);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new EquipmentRegisteredEvent(equipment.Id, equipment.RoomId, equipment.EquipmentType));

        return equipment;
    }

    public async Task<Equipment?> Handle(RegisterIncidentCommand command)
    {
        var equipment = await equipmentRepository.FindByIdWithDetailsAsync(command.EquipmentId);
        if (equipment is null) throw new InvalidOperationException($"Equipment {command.EquipmentId} not found.");

        var incident = equipment.ReportIncident(command.Description, command.Severity);
        
        equipmentRepository.Update(equipment);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new IncidentReportedEvent(equipment.Id, incident.Id, incident.Severity));

        return equipment;
    }

    public async Task<Equipment?> Handle(RunEquipmentTestCommand command)
    {
        var equipment = await equipmentRepository.FindByIdWithDetailsAsync(command.EquipmentId);
        if (equipment is null) throw new InvalidOperationException($"Equipment {command.EquipmentId} not found.");

        var test = equipment.RunTest(command.Channel);
        
        equipmentRepository.Update(equipment);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new EquipmentTestedEvent(equipment.Id, test.Id, command.Channel, test.Result == Domain.Model.ValueObjects.ETestResult.Passed));

        return equipment;
    }
}