using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Queries;

namespace KinemoPlatform.API.Analytics.Domain.Services;

public interface IOperationalReportQueryService
{
    Task<OperationalReport?> Handle(GetReportByIdQuery query);
}