using KinemoPlatform.API.Analytics.Domain.Model.Aggregates;
using KinemoPlatform.API.Analytics.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Analytics.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
///     Model builder extensions for the Analytics bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    public static void ApplyAnalyticsConfiguration(this ModelBuilder builder)
    {
        // ---------------------------------------------------------------------
        // Dashboard Aggregate Root
        // ---------------------------------------------------------------------
        builder.Entity<Dashboard>().HasKey(d => d.Id);
        builder.Entity<Dashboard>().Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Dashboard>().Property(d => d.GeneratedAt).IsRequired();
        builder.Entity<Dashboard>().Property(d => d.PeriodStart).IsRequired();
        builder.Entity<Dashboard>().Property(d => d.PeriodEnd).IsRequired();

        // ---------------------------------------------------------------------
        // OperationalMetric Entity
        // ---------------------------------------------------------------------
        builder.Entity<OperationalMetric>().HasKey(m => m.Id);
        builder.Entity<OperationalMetric>().Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<OperationalMetric>()
            .Property(m => m.MetricType).IsRequired()
            .HasConversion<string>().HasMaxLength(30);
        builder.Entity<OperationalMetric>().Property(m => m.MetricValue).IsRequired();
        builder.Entity<OperationalMetric>().Property(m => m.PeriodStart).IsRequired();
        builder.Entity<OperationalMetric>().Property(m => m.PeriodEnd).IsRequired();

        builder.Entity<Dashboard>()
            .HasMany(d => d.Metrics)
            .WithOne()
            .HasForeignKey(m => m.DashboardId)
            .HasConstraintName("fk_metrics_dashboards")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // RoomPerformance Entity
        // ---------------------------------------------------------------------
        builder.Entity<RoomPerformance>().HasKey(p => p.Id);
        builder.Entity<RoomPerformance>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<RoomPerformance>().Property(p => p.RoomId).IsRequired();
        builder.Entity<RoomPerformance>().Property(p => p.OperatingHours).IsRequired();
        builder.Entity<RoomPerformance>().Property(p => p.TotalShows).IsRequired();
        builder.Entity<RoomPerformance>().Property(p => p.IncidentCount).IsRequired();

        builder.Entity<Dashboard>()
            .HasMany(d => d.RoomPerformances)
            .WithOne()
            .HasForeignKey(p => p.DashboardId)
            .HasConstraintName("fk_room_perf_dashboards")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // IncidentSummary Entity
        // ---------------------------------------------------------------------
        builder.Entity<IncidentSummary>().HasKey(i => i.Id);
        builder.Entity<IncidentSummary>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<IncidentSummary>().Property(i => i.TotalIncidents).IsRequired();
        builder.Entity<IncidentSummary>().Property(i => i.LowCount).IsRequired();
        builder.Entity<IncidentSummary>().Property(i => i.MediumCount).IsRequired();
        builder.Entity<IncidentSummary>().Property(i => i.HighCount).IsRequired();
        builder.Entity<IncidentSummary>().Property(i => i.CriticalCount).IsRequired();

        builder.Entity<Dashboard>()
            .HasMany(d => d.IncidentSummaries)
            .WithOne()
            .HasForeignKey(i => i.DashboardId)
            .HasConstraintName("fk_incident_sum_dashboards")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // OperationalReport Aggregate Root
        // ---------------------------------------------------------------------
        builder.Entity<OperationalReport>().HasKey(r => r.Id);
        builder.Entity<OperationalReport>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<OperationalReport>()
            .Property(r => r.ReportType).IsRequired()
            .HasConversion<string>().HasMaxLength(30);
        builder.Entity<OperationalReport>().Property(r => r.GeneratedAt).IsRequired();
        builder.Entity<OperationalReport>().Property(r => r.PeriodStart).IsRequired();
        builder.Entity<OperationalReport>().Property(r => r.PeriodEnd).IsRequired();
        builder.Entity<OperationalReport>()
            .Property(r => r.ExportFormat).IsRequired()
            .HasConversion<string>().HasMaxLength(10);
    }
}