using KinemoPlatform.API.Maintenance.Domain.Model.Aggregates;
using KinemoPlatform.API.Maintenance.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Maintenance.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
///     Model builder extensions for the Maintenance bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    public static void ApplyMaintenanceConfiguration(this ModelBuilder builder)
    {
        // ---------------------------------------------------------------------
        // Equipment Aggregate Root
        // ---------------------------------------------------------------------
        builder.Entity<Equipment>().HasKey(e => e.Id);
        builder.Entity<Equipment>().Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Equipment>().Property(e => e.RoomId).IsRequired();
        builder.Entity<Equipment>().Property(e => e.Name).IsRequired().HasMaxLength(150);
        builder.Entity<Equipment>()
            .Property(e => e.EquipmentType).IsRequired()
            .HasConversion<string>().HasMaxLength(30);
        builder.Entity<Equipment>()
            .Property(e => e.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(30);
        builder.Entity<Equipment>().Property(e => e.InstallationDate).IsRequired();
        builder.Entity<Equipment>().Property(e => e.UsefulLifeHours).IsRequired();
        builder.Entity<Equipment>().Property(e => e.AccumulatedHours).IsRequired();

        // ---------------------------------------------------------------------
        // Incident Entity
        // ---------------------------------------------------------------------
        builder.Entity<Incident>().HasKey(i => i.Id);
        builder.Entity<Incident>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Incident>().Property(i => i.Description).IsRequired().HasMaxLength(1000);
        builder.Entity<Incident>()
            .Property(i => i.Severity).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<Incident>()
            .Property(i => i.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<Incident>().Property(i => i.ReportedAt).IsRequired();
        builder.Entity<Incident>().Property(i => i.ResolvedAt);

        builder.Entity<Equipment>()
            .HasMany(e => e.Incidents)
            .WithOne()
            .HasForeignKey(i => i.EquipmentId)
            .HasConstraintName("fk_incidents_equipment")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // EquipmentTest Entity
        // ---------------------------------------------------------------------
        builder.Entity<EquipmentTest>().HasKey(t => t.Id);
        builder.Entity<EquipmentTest>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<EquipmentTest>().Property(t => t.TestDate).IsRequired();
        builder.Entity<EquipmentTest>()
            .Property(t => t.Result).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<EquipmentTest>().Property(t => t.Observations).IsRequired().HasMaxLength(1000);

        builder.Entity<Equipment>()
            .HasMany(e => e.Tests)
            .WithOne()
            .HasForeignKey(t => t.EquipmentId)
            .HasConstraintName("fk_equipment_tests_equipment")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // ChannelTest Entity
        // ---------------------------------------------------------------------
        builder.Entity<ChannelTest>().HasKey(c => c.Id);
        builder.Entity<ChannelTest>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ChannelTest>()
            .Property(c => c.EffectChannel).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<ChannelTest>().Property(c => c.Intensity).IsRequired();
        builder.Entity<ChannelTest>().Property(c => c.Successful).IsRequired();

        builder.Entity<EquipmentTest>()
            .HasMany(t => t.ChannelTests)
            .WithOne()
            .HasForeignKey(c => c.EquipmentTestId)
            .HasConstraintName("fk_channel_tests_equipment_tests")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // MaintenanceOrder Aggregate Root
        // ---------------------------------------------------------------------
        builder.Entity<MaintenanceOrder>().HasKey(m => m.Id);
        builder.Entity<MaintenanceOrder>().Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<MaintenanceOrder>().Property(m => m.EquipmentId).IsRequired();
        builder.Entity<MaintenanceOrder>().Property(m => m.IncidentId);
        builder.Entity<MaintenanceOrder>()
            .Property(m => m.MaintenanceType).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<MaintenanceOrder>()
            .Property(m => m.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<MaintenanceOrder>().Property(m => m.ScheduledDate).IsRequired();
        builder.Entity<MaintenanceOrder>().Property(m => m.CompletedAt);
        builder.Entity<MaintenanceOrder>().Property(m => m.CompletionNotes).HasMaxLength(1000);
    }
}