using KinemoPlatform.API.Seats.Domain.Model.Aggregates;
using KinemoPlatform.API.Seats.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Seats.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
///     Model builder extensions for the Seats bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    public static void ApplySeatsConfiguration(this ModelBuilder builder)
    {
        // ---------------------------------------------------------------------
        // SeatAllocation Aggregate Root
        // ---------------------------------------------------------------------
        builder.Entity<SeatAllocation>().HasKey(a => a.Id);
        builder.Entity<SeatAllocation>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SeatAllocation>().Property(a => a.ShowId).IsRequired();

        // Unique index to ensure only one allocation per show
        builder.Entity<SeatAllocation>().HasIndex(a => a.ShowId).IsUnique();

        // ---------------------------------------------------------------------
        // ShowSeat Entity
        // ---------------------------------------------------------------------
        builder.Entity<ShowSeat>().HasKey(s => s.Id);
        builder.Entity<ShowSeat>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ShowSeat>().Property(s => s.SeatNumber).IsRequired();
        builder.Entity<ShowSeat>().Property(s => s.Sold).IsRequired();
        builder.Entity<ShowSeat>().Property(s => s.Enabled).IsRequired();
        builder.Entity<ShowSeat>()
            .Property(s => s.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);

        builder.Entity<SeatAllocation>()
            .HasMany(a => a.ShowSeats)
            .WithOne()
            .HasForeignKey(s => s.SeatAllocationId)
            .HasConstraintName("fk_show_seats_seat_allocations")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // ManualIntervention Entity
        // ---------------------------------------------------------------------
        builder.Entity<ManualIntervention>().HasKey(i => i.Id);
        builder.Entity<ManualIntervention>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ManualIntervention>().Property(i => i.SeatNumber).IsRequired();
        builder.Entity<ManualIntervention>().Property(i => i.OperatorId).IsRequired();
        builder.Entity<ManualIntervention>().Property(i => i.Reason).IsRequired().HasMaxLength(500);
        builder.Entity<ManualIntervention>().Property(i => i.InterventionDate).IsRequired();

        builder.Entity<SeatAllocation>()
            .HasMany(a => a.Interventions)
            .WithOne()
            .HasForeignKey(i => i.SeatAllocationId)
            .HasConstraintName("fk_manual_interventions_seat_allocations")
            .OnDelete(DeleteBehavior.Cascade);
    }
}