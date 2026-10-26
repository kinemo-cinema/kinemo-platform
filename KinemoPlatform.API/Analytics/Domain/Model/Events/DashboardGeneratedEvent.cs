using KinemoPlatform.API.Shared.Domain.Model.Events;

namespace KinemoPlatform.API.Analytics.Domain.Model.Events;

public record DashboardGeneratedEvent(int DashboardId, DateTimeOffset PeriodStart, DateTimeOffset PeriodEnd) : IEvent;