using Cortex.Mediator;
using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Commands;
using KinemoPlatform.API.Analytics.Domain.Model.Events;
using KinemoPlatform.API.Analytics.Domain.Repositories;
using KinemoPlatform.API.Analytics.Domain.Services;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Analytics.Application.Internal.CommandServices;

public class OperationalReportCommandService(
    IOperationalReportRepository reportRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IOperationalReportCommandService
{
    public async Task<OperationalReport?> Handle(GenerateReportCommand command)
    {
        var report = new OperationalReport(command);
        
        await reportRepository.AddAsync(report);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new ReportGeneratedEvent(report.Id, report.ReportType, report.ExportFormat));

        return report;
    }
}