using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Commands;

namespace KinemoPlatform.API.Analytics.Domain.Services;

public interface IDashboardCommandService
{
    /// <summary>
    ///     Handles the generation of a new operational dashboard.
    /// </summary>
    Task<Dashboard?> Handle(GenerateDashboardCommand command);
}