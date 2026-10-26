using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace KinemoPlatform.API.Analytics.Infrastructure.Persistence.EFC.Repositories;

public class OperationalReportRepository(AppDbContext context) 
    : BaseRepository<OperationalReport>(context), IOperationalReportRepository
{
    // Standard BaseRepository methods are sufficient
}