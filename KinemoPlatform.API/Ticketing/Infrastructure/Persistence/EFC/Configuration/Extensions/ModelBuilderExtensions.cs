using KinemoPlatform.API.Ticketing.Domain.Model.Aggregates;
using KinemoPlatform.API.Ticketing.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Ticketing.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
///     Model builder extensions for the Ticketing bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    public static void ApplyTicketingConfiguration(this ModelBuilder builder)
    {
        // ---------------------------------------------------------------------
        // TicketingIntegration Aggregate Root
        // ---------------------------------------------------------------------
        builder.Entity<TicketingIntegration>().HasKey(t => t.Id);
        builder.Entity<TicketingIntegration>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<TicketingIntegration>().Property(t => t.CinemaId).IsRequired();
        builder.Entity<TicketingIntegration>().Property(t => t.ProviderName).IsRequired().HasMaxLength(150);
        builder.Entity<TicketingIntegration>()
            .Property(t => t.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<TicketingIntegration>().Property(t => t.LastConnectionAt);

        // ---------------------------------------------------------------------
        // SyncLog Entity
        // ---------------------------------------------------------------------
        builder.Entity<SyncLog>().HasKey(s => s.Id);
        builder.Entity<SyncLog>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SyncLog>().Property(s => s.ShowId).IsRequired();
        builder.Entity<SyncLog>().Property(s => s.ExecutedAt).IsRequired();
        builder.Entity<SyncLog>()
            .Property(s => s.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<SyncLog>().Property(s => s.ErrorDetails).HasMaxLength(1000);

        builder.Entity<TicketingIntegration>()
            .HasMany(t => t.SyncLogs)
            .WithOne()
            .HasForeignKey(s => s.IntegrationId)
            .HasConstraintName("fk_sync_logs_integrations")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // ShowOccupancy Entity
        // ---------------------------------------------------------------------
        builder.Entity<ShowOccupancy>().HasKey(o => o.Id);
        builder.Entity<ShowOccupancy>().Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ShowOccupancy>().Property(o => o.ShowId).IsRequired();
        builder.Entity<ShowOccupancy>().Property(o => o.TotalSeats).IsRequired();
        builder.Entity<ShowOccupancy>().Property(o => o.OccupiedSeats).IsRequired();
        builder.Entity<ShowOccupancy>().Property(o => o.SynchronizedAt).IsRequired();

        builder.Entity<TicketingIntegration>()
            .HasMany(t => t.Occupancies)
            .WithOne()
            .HasForeignKey(o => o.IntegrationId)
            .HasConstraintName("fk_show_occupancies_integrations")
            .OnDelete(DeleteBehavior.Cascade);
    }
}