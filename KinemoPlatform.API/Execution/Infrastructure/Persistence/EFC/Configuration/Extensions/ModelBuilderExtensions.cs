using KinemoPlatform.API.Execution.Domain.Model.Aggregates;
using KinemoPlatform.API.Execution.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Execution.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
///     Model builder extensions for the Execution bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    public static void ApplyExecutionConfiguration(this ModelBuilder builder)
    {
        // ---------------------------------------------------------------------
        // ShowExecution Aggregate Root
        // ---------------------------------------------------------------------
        builder.Entity<ShowExecution>().HasKey(e => e.Id);
        builder.Entity<ShowExecution>().Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ShowExecution>().Property(e => e.ShowId).IsRequired();
        builder.Entity<ShowExecution>().Property(e => e.SensoryContentId).IsRequired();
        builder.Entity<ShowExecution>()
            .Property(e => e.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(30);
        builder.Entity<ShowExecution>().Property(e => e.StartedAt).IsRequired();
        builder.Entity<ShowExecution>().Property(e => e.FinishedAt);

        // ---------------------------------------------------------------------
        // SensorySequence Entity
        // ---------------------------------------------------------------------
        builder.Entity<SensorySequence>().HasKey(s => s.Id);
        builder.Entity<SensorySequence>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SensorySequence>().Property(s => s.SensoryTrackId).IsRequired();
        builder.Entity<SensorySequence>().Property(s => s.CurrentTimeCode).IsRequired();
        builder.Entity<SensorySequence>().Property(s => s.ActualTimeCode).IsRequired();
        builder.Entity<SensorySequence>()
            .Property(s => s.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);

        builder.Entity<ShowExecution>()
            .HasMany(e => e.Sequences)
            .WithOne()
            .HasForeignKey(s => s.ExecutionId)
            .HasConstraintName("fk_sensory_sequences_executions")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // SynchronizationEvent Entity
        // ---------------------------------------------------------------------
        builder.Entity<SynchronizationEvent>().HasKey(s => s.Id);
        builder.Entity<SynchronizationEvent>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SynchronizationEvent>().Property(s => s.ExpectedTimeCode).IsRequired();
        builder.Entity<SynchronizationEvent>().Property(s => s.ActualTimeCode).IsRequired();
        builder.Entity<SynchronizationEvent>().Property(s => s.DriftMilliseconds).IsRequired();
        builder.Entity<SynchronizationEvent>().Property(s => s.OccurredAt).IsRequired();
        builder.Entity<SynchronizationEvent>().Property(s => s.EventType).IsRequired().HasMaxLength(100);

        builder.Entity<ShowExecution>()
            .HasMany(e => e.SyncEvents)
            .WithOne()
            .HasForeignKey(s => s.ExecutionId)
            .HasConstraintName("fk_sync_events_executions")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // HardwareExecutionLog Entity
        // ---------------------------------------------------------------------
        builder.Entity<HardwareExecutionLog>().HasKey(h => h.Id);
        builder.Entity<HardwareExecutionLog>().Property(h => h.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<HardwareExecutionLog>().Property(h => h.OccurredAt).IsRequired();
        builder.Entity<HardwareExecutionLog>().Property(h => h.Message).IsRequired().HasMaxLength(1000);
        builder.Entity<HardwareExecutionLog>().Property(h => h.Severity).IsRequired().HasMaxLength(50);

        builder.Entity<ShowExecution>()
            .HasMany(e => e.HardwareLogs)
            .WithOne()
            .HasForeignKey(h => h.ExecutionId)
            .HasConstraintName("fk_hardware_logs_executions")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // EmergencyEvent Entity
        // ---------------------------------------------------------------------
        builder.Entity<EmergencyEvent>().HasKey(ev => ev.Id);
        builder.Entity<EmergencyEvent>().Property(ev => ev.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<EmergencyEvent>().Property(ev => ev.Reason).IsRequired().HasMaxLength(500);
        builder.Entity<EmergencyEvent>().Property(ev => ev.TriggeredAt).IsRequired();
        builder.Entity<EmergencyEvent>().Property(ev => ev.ResolvedAt);
        builder.Entity<EmergencyEvent>()
            .Property(ev => ev.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);

        builder.Entity<ShowExecution>()
            .HasMany(e => e.EmergencyEvents)
            .WithOne()
            .HasForeignKey(ev => ev.ExecutionId)
            .HasConstraintName("fk_emergency_events_executions")
            .OnDelete(DeleteBehavior.Cascade);
    }
}