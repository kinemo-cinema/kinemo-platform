using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Scheduling.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Scheduling.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
///     Model builder extensions for the Scheduling bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    public static void ApplySchedulingConfiguration(this ModelBuilder builder)
    {
        // ---------------------------------------------------------------------
        // Show Aggregate Root
        // ---------------------------------------------------------------------
        builder.Entity<Show>().HasKey(s => s.Id);
        builder.Entity<Show>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Show>().Property(s => s.MovieId).IsRequired();
        builder.Entity<Show>().Property(s => s.RoomId).IsRequired();
        builder.Entity<Show>()
            .Property(s => s.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);

        // Map the TimeSlot Value Object (Owned Entity)
        builder.Entity<Show>().OwnsOne(s => s.TimeSlot, ts =>
        {
            ts.Property(t => t.StartDateTime).IsRequired().HasColumnName("start_datetime");
            ts.Property(t => t.EndDateTime).IsRequired().HasColumnName("end_datetime");
        });

        // ---------------------------------------------------------------------
        // ScheduleConflict Entity
        // ---------------------------------------------------------------------
        builder.Entity<ScheduleConflict>().HasKey(sc => sc.Id);
        builder.Entity<ScheduleConflict>().Property(sc => sc.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ScheduleConflict>().Property(sc => sc.ShowId).IsRequired();
        builder.Entity<ScheduleConflict>().Property(sc => sc.ConflictingShowId).IsRequired();
        builder.Entity<ScheduleConflict>().Property(sc => sc.Description).IsRequired().HasMaxLength(500);
        builder.Entity<ScheduleConflict>().Property(sc => sc.DetectedAt).IsRequired();
        builder.Entity<ScheduleConflict>().Property(sc => sc.Resolved).IsRequired();
        builder.Entity<ScheduleConflict>().Property(sc => sc.ResolvedAt);

        // Foreign Keys for ScheduleConflict
        builder.Entity<ScheduleConflict>()
            .HasOne<Show>()
            .WithMany()
            .HasForeignKey(sc => sc.ShowId)
            .HasConstraintName("fk_schedule_conflicts_shows_show_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ScheduleConflict>()
            .HasOne<Show>()
            .WithMany()
            .HasForeignKey(sc => sc.ConflictingShowId)
            .HasConstraintName("fk_schedule_conflicts_shows_conflicting_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}