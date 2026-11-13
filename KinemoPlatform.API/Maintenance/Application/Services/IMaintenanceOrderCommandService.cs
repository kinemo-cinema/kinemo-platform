using KinemoPlatform.API.Maintenance.Application.Errors;
using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Commands;
using KinemoPlatform.API.Shared.Application.Patterns;

namespace KinemoPlatform.API.Maintenance.Application.Services;

/// <summary>
///     Represents the maintenance order command service in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Every operation returns a success result with the affected <see cref="MaintenanceOrder" />, or a failure with
///     an <see cref="EMaintenanceOrderError" />.
/// </remarks>
public interface IMaintenanceOrderCommandService
{
    /// <summary>
    ///     Handles scheduling a preventive or corrective maintenance order (US23).
    /// </summary>
    Task<Result<MaintenanceOrder, EMaintenanceOrderError>> Handle(ScheduleMaintenanceCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles completing a maintenance order and resolving its incident (US21).
    /// </summary>
    Task<Result<MaintenanceOrder, EMaintenanceOrderError>> Handle(CompleteMaintenanceCommand command,
        CancellationToken cancellationToken = default);
}
