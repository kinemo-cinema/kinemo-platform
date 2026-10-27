using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Movies.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Movies.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
///     Model builder extensions for the Movies bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Apply the EF Core configuration for the Movies bounded context (BC01).
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="ModelBuilder" /> to configure.
    /// </param>
    public static void ApplyMoviesConfiguration(this ModelBuilder builder)
    {
        // ---------------------------------------------------------------------
        // Movie aggregate root
        // ---------------------------------------------------------------------
        builder.Entity<Movie>().HasKey(m => m.Id);
        builder.Entity<Movie>().Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Movie>().Property(m => m.Title).IsRequired().HasMaxLength(200);
        builder.Entity<Movie>().Property(m => m.DurationMinutes).IsRequired();
        builder.Entity<Movie>()
            .Property(m => m.Genre).IsRequired()
            .HasConversion<string>().HasMaxLength(30);
        builder.Entity<Movie>()
            .Property(m => m.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);

        builder.Entity<Movie>().HasIndex(m => m.Title).IsUnique();
        builder.Entity<Movie>().HasIndex(m => m.Genre);
        builder.Entity<Movie>().HasIndex(m => m.Status);

        // ---------------------------------------------------------------------
        // SensoryContent (1-to-1 with Movie)
        // ---------------------------------------------------------------------
        builder.Entity<SensoryContent>().HasKey(sc => sc.Id);
        builder.Entity<SensoryContent>().Property(sc => sc.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SensoryContent>()
            .Property(sc => sc.IntensityLevel).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<SensoryContent>()
            .Property(sc => sc.Status).IsRequired()
            .HasConversion<string>().HasMaxLength(20);

        builder.Entity<Movie>()
            .HasOne(m => m.SensoryContent)
            .WithOne()
            .HasForeignKey<SensoryContent>(sc => sc.MovieId)
            .HasConstraintName("fk_sensory_contents_movies")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // SensoryTrack (many per SensoryContent)
        // ---------------------------------------------------------------------
        builder.Entity<SensoryTrack>().HasKey(t => t.Id);
        builder.Entity<SensoryTrack>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SensoryTrack>()
            .Property(t => t.EffectType).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<SensoryTrack>().Property(t => t.FilePath).IsRequired().HasMaxLength(500);
        builder.Entity<SensoryTrack>().Property(t => t.DurationSeconds).IsRequired();

        builder.Entity<SensoryContent>()
            .HasMany(sc => sc.Tracks)
            .WithOne()
            .HasForeignKey(t => t.SensoryContentId)
            .HasConstraintName("fk_sensory_tracks_sensory_contents")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------------------------------------------------------------
        // ConfigurationHistoryEntry (many per SensoryContent)
        // ---------------------------------------------------------------------
        builder.Entity<ConfigurationHistoryEntry>().HasKey(h => h.Id);
        builder.Entity<ConfigurationHistoryEntry>().Property(h => h.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ConfigurationHistoryEntry>()
            .Property(h => h.PreviousIntensity).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<ConfigurationHistoryEntry>()
            .Property(h => h.NewIntensity).IsRequired()
            .HasConversion<string>().HasMaxLength(20);
        builder.Entity<ConfigurationHistoryEntry>().Property(h => h.ChangedAt).IsRequired();

        builder.Entity<SensoryContent>()
            .HasMany(sc => sc.History)
            .WithOne()
            .HasForeignKey(h => h.SensoryContentId)
            .HasConstraintName("fk_configuration_history_sensory_contents")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}