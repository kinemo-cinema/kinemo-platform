using KinemoPlatform.API.Analytics.Domain.Model.Commands;
using KinemoPlatform.API.Analytics.Domain.Model.Entities;

namespace KinemoPlatform.API.Analytics.Domain.Model.Aggregates;

/// <summary>
///     Dashboard aggregate root.
/// </summary>
/// <remarks>
///     Aggregates operational metrics, room performance, and incident summaries
///     for a specific time period.
/// </remarks>
public partial class Dashboard
{
    public Dashboard()
    {
        Metrics = new List<OperationalMetric>();
        RoomPerformances = new List<RoomPerformance>();
        IncidentSummaries = new List<IncidentSummary>();
    }

    public Dashboard(GenerateDashboardCommand command) : this()
    {
        PeriodStart = command.PeriodStart;
        PeriodEnd = command.PeriodEnd;
        GeneratedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; }
    public DateTimeOffset GeneratedAt { get; private set; }
    public DateTimeOffset PeriodStart { get; private set; }
    public DateTimeOffset PeriodEnd { get; private set; }

    public ICollection<OperationalMetric> Metrics { get; }
    public ICollection<RoomPerformance> RoomPerformances { get; }
    public ICollection<IncidentSummary> IncidentSummaries { get; }

    // Methods to populate the read models (usually called by a Data Gatherer service)
    public void AddMetric(OperationalMetric metric) => Metrics.Add(metric);
    public void AddRoomPerformance(RoomPerformance performance) => RoomPerformances.Add(performance);
    public void AddIncidentSummary(IncidentSummary summary) => IncidentSummaries.Add(summary);
}