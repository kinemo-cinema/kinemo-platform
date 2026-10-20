using KinemoPlatform.API.Movies.Infrastructure.Persistence.EFC.Configuration.Extensions;

// TODO: Uncomment as you build other bounded contexts
// using KinemoPlatform.API.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;              // IAM
// using KinemoPlatform.API.Subscriptions.Infrastructure.Persistence.EFC.Configuration.Extensions;    // BC09
using KinemoPlatform.API.Scheduling.Infrastructure.Persistence.EFC.Configuration.Extensions;       // BC02
using KinemoPlatform.API.Rooms.Infrastructure.Persistence.EFC.Configuration.Extensions;            // BC03
// using KinemoPlatform.API.Ticketing.Infrastructure.Persistence.EFC.Configuration.Extensions;        // BC04
// using KinemoPlatform.API.Seats.Infrastructure.Persistence.EFC.Configuration.Extensions;            // BC05
// using KinemoPlatform.API.Execution.Infrastructure.Persistence.EFC.Configuration.Extensions;        // BC06
// using KinemoPlatform.API.Maintenance.Infrastructure.Persistence.EFC.Configuration.Extensions;      // BC07
// using KinemoPlatform.API.Analytics.Infrastructure.Persistence.EFC.Configuration.Extensions;        // BC08

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

        // Movies Bounded Context (BC01)
        builder.ApplyMoviesConfiguration();
        
        // Scheduling Bounded Context (BC02)
        builder.ApplySchedulingConfiguration();
       
        // Scheduling Bounded Context (BC03)
        builder.ApplyRoomsConfiguration();

        // TODO: Uncomment as you build other bounded contexts
        // builder.ApplyIAMConfiguration();              // IAM
        // builder.ApplySubscriptionsConfiguration();    // BC09
        
        builder.ApplyRoomsConfiguration();            // BC03
        // builder.ApplyTicketingConfiguration();        // BC04
        // builder.ApplySeatsConfiguration();            // BC05
        // builder.ApplyExecutionConfiguration();        // BC06
        // builder.ApplyMaintenanceConfiguration();      // BC07
        // builder.ApplyAnalyticsConfiguration();        // BC08

        // General Naming Convention for the database objects
        builder.UseSnakeCaseNamingConvention();
    }
}