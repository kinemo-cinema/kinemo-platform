using KinemoPlatform.API.Analytics.Application.Errors;
using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Commands;
using KinemoPlatform.API.Shared.Application.Patterns;

namespace KinemoPlatform.API.Analytics.Application.Services;

public interface IOperationalReportCommandService
{
    /// <summary>
    ///     Handles the generation of a new operational report.
    /// </summary>
    Task<Result<OperationalReport, EAnalyticsError>> Handle(
        GenerateReportCommand command,
        CancellationToken cancellationToken = default);
}