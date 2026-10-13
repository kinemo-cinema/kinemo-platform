// TODO: Uncomment these usings as you build your other bounded contexts
// using KinemoPlatform.API.Profiles.Infrastructure.Persistence.EFC.Configuration.Extensions;
// using KinemoPlatform.API.Publishing.Infrastructure.Persistence.EFC.Configuration.Extensions;
// using KinemoPlatform.API.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;

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

        // TODO: Apply Publishing configuration when available
        // builder.ApplyPublishingConfiguration();

        // TODO: Apply Profiles configuration when available
        // builder.ApplyProfilesConfiguration();
        
        // TODO: Apply IAM configuration when available
        // builder.ApplyIAMConfiguration();
        
        // General Naming Convention for the database objects
        builder.UseSnakeCaseNamingConvention();
    }
}