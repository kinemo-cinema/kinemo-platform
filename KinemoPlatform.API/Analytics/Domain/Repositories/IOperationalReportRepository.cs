using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Analytics.Domain.Repositories;

/// <summary>
///     Represents the operational report repository in the Kinemo Platform.
/// </summary>
public interface IOperationalReportRepository : IBaseRepository<OperationalReport>
{
}