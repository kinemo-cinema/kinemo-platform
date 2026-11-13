using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql;

namespace KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

// A distinct context keeps PostgreSQL migrations separate from the existing MySQL history.
public class PostgresAppDbContext(DbContextOptions<PostgresAppDbContext> options) : AppDbContext(options)
{
    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);
        builder.Properties<DateTimeOffset>().HaveConversion<UtcDateTimeOffsetConverter>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<StoredSensoryBlob>(entity =>
        {
            entity.ToTable("stored_sensory_blobs");
            entity.HasKey(blob => blob.Path);
            entity.Property(blob => blob.Path).HasColumnName("path").HasMaxLength(512);
            entity.Property(blob => blob.Content).HasColumnName("content");
        });
    }

    public static string NormalizeConnectionString(string value)
    {
        if (!value.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) &&
            !value.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase)) return value;
        var uri = new Uri(value);
        var credentials = uri.UserInfo.Split(':', 2);
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort ? 5432 : uri.Port,
            Database = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/')),
            Username = Uri.UnescapeDataString(credentials[0]),
            Password = credentials.Length > 1 ? Uri.UnescapeDataString(credentials[1]) : "",
            MaxPoolSize = 10,
            SslMode = uri.Host.EndsWith(".render.com", StringComparison.OrdinalIgnoreCase) ? SslMode.Require : SslMode.Prefer
        };
        return builder.ConnectionString;
    }
}

public class UtcDateTimeOffsetConverter() : ValueConverter<DateTimeOffset, DateTimeOffset>(
    value => value.ToUniversalTime(), value => value);

public class StoredSensoryBlob
{
    public string Path { get; set; } = "";
    public byte[] Content { get; set; } = [];
}

public class PostgresDesignTimeFactory : IDesignTimeDbContextFactory<PostgresAppDbContext>
{
    public PostgresAppDbContext CreateDbContext(string[] args) => new(
        new DbContextOptionsBuilder<PostgresAppDbContext>()
            .UseNpgsql("Host=localhost;Database=kinemo;Username=kinemo")
            .Options);
}
