using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Commands;

namespace KinemoPlatform.API.Analytics.Application.Services;

public interface IDashboardCommandService
{
    /// <summary>
    ///     Handles the generation of a new operational dashboard.
    /// </summary>
    Task<Dashboard?> Handle(
        GenerateDashboardCommand command,
        CancellationToken cancellationToken = default);
}