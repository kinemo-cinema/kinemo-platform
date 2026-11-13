using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

// Optional one-time transfer into a NEW cloud database. Never merges over existing records.
public static class PostgresBootstrap
{
    public static async Task ImportAsync(PostgresAppDbContext db, string? snapshot, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(snapshot)) return;
        var compressed = Convert.FromBase64String(snapshot);
        var fingerprint = Convert.ToHexString(SHA256.HashData(compressed));
        using var input = new GZipStream(new MemoryStream(compressed), CompressionMode.Decompress);
        var tables = (await JsonNode.ParseAsync(input))!.AsObject();
        var allowed = db.Model.GetRelationalModel().Tables.Select(t => t.Name).ToHashSet();
        if (tables.Any(t => !allowed.Contains(t.Key))) throw new InvalidOperationException("Unknown snapshot table.");
        static string Quote(string identifier) => new NpgsqlCommandBuilder().QuoteIdentifier(identifier);
        await db.Database.OpenConnectionAsync();
        var connection = (NpgsqlConnection)db.Database.GetDbConnection();
        await using var transaction = await connection.BeginTransactionAsync();
        async Task<object?> Execute(string sql, params NpgsqlParameter[] parameters)
        {
            await using var command = new NpgsqlCommand(sql, connection, transaction);
            command.Parameters.AddRange(parameters);
            return await command.ExecuteScalarAsync();
        }
        await Execute("CREATE TABLE IF NOT EXISTS kinemo_imports (fingerprint text PRIMARY KEY, imported_at timestamptz NOT NULL DEFAULT now())");
        if (await Execute("SELECT fingerprint FROM kinemo_imports WHERE fingerprint = @fingerprint",
                new NpgsqlParameter("fingerprint", fingerprint)) is not null)
        {
            await transaction.CommitAsync();
            return;
        }
        foreach (var table in allowed)
            if (Convert.ToInt64(await Execute($"SELECT count(*) FROM {Quote(table)}")) != 0)
                throw new InvalidOperationException("Snapshot import requires an empty database; existing data will not be overwritten.");

        // Order all tables by their real PostgreSQL foreign keys, including EF owned entities.
        var dependencies = allowed.ToDictionary(t => t, _ => new HashSet<string>());
        await using (var command = new NpgsqlCommand("SELECT c.relname, p.relname FROM pg_constraint f JOIN pg_class c ON c.oid=f.conrelid JOIN pg_class p ON p.oid=f.confrelid WHERE f.contype='f' AND c.relnamespace='public'::regnamespace", connection, transaction))
        await using (var reader = await command.ExecuteReaderAsync())
            while (await reader.ReadAsync())
            {
                var child = reader.GetString(0); var parent = reader.GetString(1);
                if (child != parent && dependencies.ContainsKey(child) && allowed.Contains(parent)) dependencies[child].Add(parent);
            }
        var imported = new HashSet<string>();
        var rowCount = 0;
        while (imported.Count < allowed.Count)
        {
            var ready = allowed.Where(t => !imported.Contains(t) && dependencies[t].All(imported.Contains)).ToArray();
            if (ready.Length == 0) throw new InvalidOperationException("Snapshot contains cyclic foreign keys.");
            foreach (var table in ready)
            {
                if (tables[table] is JsonArray rows)
                    foreach (var row in rows)
                    {
                        await Execute($"INSERT INTO {Quote(table)} SELECT * FROM jsonb_populate_record(NULL::{Quote(table)}, @row)",
                            new NpgsqlParameter("row", NpgsqlDbType.Jsonb) { Value = row!.ToJsonString() });
                        rowCount++;
                    }
                var metadata = db.Model.GetRelationalModel().Tables.Single(t => t.Name == table);
                foreach (var column in metadata.Columns.Where(c => c.PropertyMappings.Any(p => p.Property.ValueGenerated == Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd)))
                {
                    var sequence = await Execute("SELECT pg_get_serial_sequence(@table, @column)",
                        new NpgsqlParameter("table", Quote(table)), new NpgsqlParameter("column", column.Name));
                    if (sequence is string sequenceName)
                        await Execute($"SELECT setval(@sequence::regclass, GREATEST(COALESCE(MAX({Quote(column.Name)}), 0), 1), MAX({Quote(column.Name)}) IS NOT NULL) FROM {Quote(table)}",
                            new NpgsqlParameter("sequence", sequenceName));
                }
                imported.Add(table);
            }
        }
        await Execute("INSERT INTO kinemo_imports(fingerprint) VALUES (@fingerprint)", new NpgsqlParameter("fingerprint", fingerprint));
        await transaction.CommitAsync();
        logger.LogInformation("Imported {RowCount} existing records into the new PostgreSQL database.", rowCount);
    }
}
