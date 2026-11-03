using KinemoPlatform.API.Analytics.Application.Errors;
using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Commands;
using KinemoPlatform.API.Shared.Application.Patterns;

namespace KinemoPlatform.API.Analytics.Application.Services;

public interface IDashboardCommandService
{
    /// <summary>
    ///     Handles the generation of a new operational dashboard.
    /// </summary>
    Task<Result<Dashboard, EAnalyticsError>> Handle(
        GenerateDashboardCommand command,
        CancellationToken cancellationToken = default);
}