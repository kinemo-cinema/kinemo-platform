// BC01: Movies
using KinemoPlatform.API.Movies.Infrastructure.Persistence.EFC.Configuration.Extensions;
// BC02: Scheduling
using KinemoPlatform.API.Scheduling.Infrastructure.Persistence.EFC.Configuration.Extensions;
// BC03: Rooms
using KinemoPlatform.API.Rooms.Infrastructure.Persistence.EFC.Configuration.Extensions;
// BC04: Ticketing
using KinemoPlatform.API.Ticketing.Infrastructure.Persistence.EFC.Configuration.Extensions;
// BC05: Seats
// using KinemoPlatform.API.Seats.Infrastructure.Persistence.EFC.Configuration.Extensions;
// BC06: Execution
using KinemoPlatform.API.Execution.Infrastructure.Persistence.EFC.Configuration.Extensions;
// BC07: Maintenance
// using KinemoPlatform.API.Maintenance.Infrastructure.Persistence.EFC.Configuration.Extensions;
// BC08: Analytics
using KinemoPlatform.API.Analytics.Infrastructure.Persistence.EFC.Configuration.Extensions;
// BC09: Subscriptions
using KinemoPlatform.API.Subscriptions.Infrastructure.Persistence.EFC.Configuration.Extensions;

using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context for the Kinemo Platform
/// </summary>
/// <param name="options">
///     The options for the database context
/// </param>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <summary>
    ///     On configuring the database context
    /// </summary>
    /// <remarks>
    ///     This method is used to configure the database context.
    ///     It also adds the created and updated date interceptor to the database context.
    /// </remarks>
    /// <param name="builder">
    ///     The option builder for the database context
    /// </param>
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    /// <summary>
    ///     On creating the database model
    /// </summary>
    /// <remarks>
    ///     This method is used to create the database model for the application.
    /// </remarks>
    /// <param name="builder">
    ///     The model builder for the database context
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply configurations for all Bounded Contexts exactly once
        builder.ApplyMoviesConfiguration();           // BC01
        builder.ApplySchedulingConfiguration();       // BC02
        builder.ApplyRoomsConfiguration();            // BC03
        builder.ApplyTicketingConfiguration();        // BC04
        // builder.ApplySeatsConfiguration();            // BC05
        builder.ApplyExecutionConfiguration();        // BC06
        // builder.ApplyMaintenanceConfiguration();      // BC07
        builder.ApplyAnalyticsConfiguration();        // BC08
        builder.ApplySubscriptionsConfiguration();    // BC09

        // TODO: Uncomment when IAM is built
        // builder.ApplyIAMConfiguration();           // IAM

        // General Naming Convention for the database objects
        builder.UseSnakeCaseNamingConvention();
    }
}