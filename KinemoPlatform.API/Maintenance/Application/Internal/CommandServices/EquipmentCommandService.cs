using Cortex.Mediator;
using KinemoPlatform.API.Maintenance.Application.Errors;
using KinemoPlatform.API.Maintenance.Application.Services;
using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Commands;
using KinemoPlatform.API.Maintenance.Domain.Model.Events;
using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;
using KinemoPlatform.API.Maintenance.Domain.Repositories;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Maintenance.Application.Internal.CommandServices;

/// <summary>
///     Implementation of the equipment command service.
/// </summary>
/// <remarks>
///     Expected failures (equipment not found, duplicate name in a room) are returned as
///     <see cref="EEquipmentError" /> inside a <see cref="Result{TValue,TError}" />. Unexpected failures are not
///     caught here; they surface as exceptions and are turned into a ProblemDetails 500 response globally.
/// </remarks>
public class EquipmentCommandService(
    IEquipmentRepository equipmentRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IEquipmentCommandService
{
    public async Task<Result<Equipment, EEquipmentError>> Handle(RegisterEquipmentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await equipmentRepository.ExistsByNameAndRoomAsync(command.Name, command.RoomId, cancellationToken))
            return Fail(EEquipmentError.DuplicateEquipment);

        var equipment = new Equipment(command);
        await equipmentRepository.AddAsync(equipment, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new EquipmentRegisteredEvent(equipment.Id, equipment.RoomId, equipment.EquipmentType),
            cancellationToken);

        return Ok(equipment);
    }

    public async Task<Result<Equipment, EEquipmentError>> Handle(RegisterIncidentCommand command,
        CancellationToken cancellationToken = default)
    {
        var equipment = await equipmentRepository.FindByIdWithDetailsAsync(command.EquipmentId, cancellationToken);
        if (equipment is null) return Fail(EEquipmentError.EquipmentNotFound);

        var incident = equipment.ReportIncident(command.Description, command.Severity);

        equipmentRepository.Update(equipment);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new IncidentReportedEvent(equipment.Id, incident.Id, incident.Severity), cancellationToken);

        return Ok(equipment);
    }

    public async Task<Result<Equipment, EEquipmentError>> Handle(RunEquipmentTestCommand command,
        CancellationToken cancellationToken = default)
    {
        var equipment = await equipmentRepository.FindByIdWithDetailsAsync(command.EquipmentId, cancellationToken);
        if (equipment is null) return Fail(EEquipmentError.EquipmentNotFound);

        var test = equipment.RunTest(command.Channel);

        equipmentRepository.Update(equipment);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new EquipmentTestedEvent(equipment.Id, test.Id, command.Channel, test.Result == ETestResult.Passed),
            cancellationToken);

        return Ok(equipment);
    }

    private static Result<Equipment, EEquipmentError> Ok(Equipment equipment) =>
        new Result<Equipment, EEquipmentError>.Success(equipment);

    private static Result<Equipment, EEquipmentError> Fail(EEquipmentError error) =>
        new Result<Equipment, EEquipmentError>.Failure(error);
}
