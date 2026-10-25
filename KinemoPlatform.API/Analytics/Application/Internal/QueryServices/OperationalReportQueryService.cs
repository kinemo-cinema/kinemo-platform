using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Queries;
using KinemoPlatform.API.Analytics.Domain.Repositories;
using KinemoPlatform.API.Analytics.Domain.Services;

namespace KinemoPlatform.API.Analytics.Application.Internal.QueryServices;

public class OperationalReportQueryService(IOperationalReportRepository reportRepository) : IOperationalReportQueryService
{
    public async Task<OperationalReport?> Handle(GetReportByIdQuery query)
    {
        return await reportRepository.FindByIdAsync(query.ReportId);
    }
}