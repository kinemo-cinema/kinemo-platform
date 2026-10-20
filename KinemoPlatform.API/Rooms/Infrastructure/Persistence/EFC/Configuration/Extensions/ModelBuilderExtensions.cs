using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Rooms.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Rooms.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
///     Model builder extensions for the Rooms bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    public static void ApplyRoomsConfiguration(this ModelBuilder builder)
    {
        // ---------------------------------------------------------------------
        // Room Aggregate Root
        // ---------------------------------------------------------------------
        builder.Entity<Room>().HasKey(r => r.Id);
        builder.Entity<Room>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Room>().Property(r => r.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Room>().Property(r => r.Capacity).IsRequired();
        builder.Entity<Room>()
            .Property(r => r.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);

        builder.Entity<Room>().HasIndex(r => r.Name).IsUnique();

        // ---------------------------------------------------------------------
        // RoomInspection Entity
        // ---------------------------------------------------------------------
        builder.Entity<RoomInspection>().HasKey(i => i.Id);
        builder.Entity<RoomInspection>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<RoomInspection>().Property(i => i.Observations).IsRequired().HasMaxLength(1000);
        builder.Entity<RoomInspection>().Property(i => i.InspectedAt).IsRequired();
        builder.Entity<RoomInspection>()
            .Property(i => i.Result).IsRequired()
            .HasConversion<string>().HasMaxLength(20);

        builder.Entity<Room>()
            .HasMany(r => r.Inspections)
            .WithOne()
            .HasForeignKey(i => i.RoomId)
            .HasConstraintName("fk_room_inspections_rooms")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // RoomBlock Entity
        // ---------------------------------------------------------------------
        builder.Entity<RoomBlock>().HasKey(b => b.Id);
        builder.Entity<RoomBlock>().Property(b => b.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<RoomBlock>().Property(b => b.Reason).IsRequired().HasMaxLength(500);
        builder.Entity<RoomBlock>().Property(b => b.StartDateTime).IsRequired();
        builder.Entity<RoomBlock>().Property(b => b.EndDateTime).IsRequired();
        builder.Entity<RoomBlock>().Property(b => b.Active).IsRequired();

        builder.Entity<Room>()
            .HasMany(r => r.Blocks)
            .WithOne()
            .HasForeignKey(b => b.RoomId)
            .HasConstraintName("fk_room_blocks_rooms")
            .OnDelete(DeleteBehavior.Cascade);
    }
}