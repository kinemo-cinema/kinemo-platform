using KinemoPlatform.API.Maintenance.Application.Errors;
using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Commands;
using KinemoPlatform.API.Shared.Application.Patterns;

namespace KinemoPlatform.API.Maintenance.Application.Services;

/// <summary>
///     Represents the equipment command service in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Every operation returns a success result with the affected <see cref="Equipment" />, or a failure with an
///     <see cref="EEquipmentError" />.
/// </remarks>
public interface IEquipmentCommandService
{
    /// <summary>
    ///     Handles the registration of a new hardware component (US19).
    /// </summary>
    Task<Result<Equipment, EEquipmentError>> Handle(RegisterEquipmentCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles reporting an incident on a piece of equipment (US20).
    /// </summary>
    Task<Result<Equipment, EEquipmentError>> Handle(RegisterIncidentCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles running a diagnostic test on a channel of a piece of equipment (US17).
    /// </summary>
    Task<Result<Equipment, EEquipmentError>> Handle(RunEquipmentTestCommand command,
        CancellationToken cancellationToken = default);
}
