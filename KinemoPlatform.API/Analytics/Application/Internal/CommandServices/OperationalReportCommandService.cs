using Cortex.Mediator;
using KinemoPlatform.API.Analytics.Application.Errors;
using KinemoPlatform.API.Analytics.Application.Services;
using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Commands;
using KinemoPlatform.API.Analytics.Domain.Model.Events;
using KinemoPlatform.API.Analytics.Domain.Repositories;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Analytics.Application.Internal.CommandServices;

public class OperationalReportCommandService(
    IOperationalReportRepository reportRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IOperationalReportCommandService
{
    public async Task<Result<OperationalReport, EAnalyticsError>> Handle(
        GenerateReportCommand command,
        CancellationToken cancellationToken = default)
    {
        var report = new OperationalReport(command);

        await reportRepository.AddAsync(report, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new ReportGeneratedEvent(
                report.Id,
                report.ReportType,
                report.ExportFormat),
            cancellationToken);

        return new Result<OperationalReport, EAnalyticsError>.Success(report);
    }
}