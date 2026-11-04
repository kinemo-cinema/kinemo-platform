using KinemoPlatform.API.Analytics.Application.Services;
using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Queries;
using KinemoPlatform.API.Analytics.Domain.Repositories;

namespace KinemoPlatform.API.Analytics.Application.Internal.QueryServices;

public class OperationalReportQueryService(IOperationalReportRepository reportRepository)
    : IOperationalReportQueryService
{
    public async Task<OperationalReport?> Handle(
        GetReportByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await reportRepository.FindByIdAsync(query.ReportId, cancellationToken);
    }
}