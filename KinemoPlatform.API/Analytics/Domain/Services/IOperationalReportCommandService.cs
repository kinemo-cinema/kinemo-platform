using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Commands;

namespace KinemoPlatform.API.Analytics.Domain.Services;

public interface IOperationalReportCommandService
{
    /// <summary>
    ///     Handles the generation of a new operational report.
    /// </summary>
    Task<OperationalReport?> Handle(GenerateReportCommand command);
}