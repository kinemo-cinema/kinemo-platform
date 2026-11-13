using System.Text;
using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;
using KinemoPlatform.API.Movies.Infrastructure.External.FileStorage;
using KinemoPlatform.API.Rooms.Domain.Model.Aggregates;
using KinemoPlatform.API.Rooms.Domain.Model.Commands;
using KinemoPlatform.API.Scheduling.Domain.Model.Aggregates;
using KinemoPlatform.API.Scheduling.Domain.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

// Opt-in deployment verification. All writes stay in a fresh, temporary PostgreSQL schema.
public static class PostgresSmokeCheck
{
    public static async Task RunAsync(string connectionString, ILogger logger)
    {
        var schema = "kinemo_verify_" + Guid.NewGuid().ToString("N");
        var quotedSchema = new NpgsqlCommandBuilder().QuoteIdentifier(schema);
        var source = new NpgsqlConnectionStringBuilder(connectionString) { MaxPoolSize = 2 };
        await using var connection = new NpgsqlConnection(source.ConnectionString);
        await connection.OpenAsync();
        await using (var create = new NpgsqlCommand($"CREATE SCHEMA {quotedSchema}", connection))
            await create.ExecuteNonQueryAsync();
        try
        {
            source.SearchPath = schema;
            var options = new DbContextOptionsBuilder<PostgresAppDbContext>()
                .UseNpgsql(source.ConnectionString, pg => pg.MigrationsHistoryTable("__EFMigrationsHistory", schema))
                .Options;
            int movieId, showId;
            string path;
            var localTime = new DateTimeOffset(2026, 12, 1, 10, 0, 0, TimeSpan.FromHours(-5));
            var content = Encoding.UTF8.GetBytes("Kinemo isolated PostgreSQL upload verification");
            await using (var db = new PostgresAppDbContext(options))
            {
                await db.Database.MigrateAsync();
                var movie = new Movie("Isolated verification", 90, EMovieGenre.SciFi);
                var room = new Room(new CreateRoomCommand("Isolated room", 40));
                db.Add(movie); db.Add(room);
                await db.SaveChangesAsync();
                if (movie.Id <= 0 || room.Id <= 0) throw new InvalidOperationException("Identity generation failed.");
                movieId = movie.Id;
                var show = new Show(new ScheduleShowCommand(movie.Id, room.Id, localTime, localTime.AddHours(2)));
                db.Add(show);
                await db.SaveChangesAsync();
                showId = show.Id;
                var storage = new PostgresSensoryFileStorage(db);
                path = await storage.UploadAsync(new MemoryStream(content), "verification.txt", movie.Id);
                movie.AssociateSensoryFile(EEffectType.Motion, path, 60);
                movie.UpdateDetails("Isolated verification edited", 95, EMovieGenre.Action);
                await db.SaveChangesAsync();
            }
            // A separate context proves persistence rather than only inspecting tracked objects.
            await using (var db = new PostgresAppDbContext(options))
            {
                var movie = await db.Set<Movie>().Include(m => m.SensoryContent).ThenInclude(s => s.Tracks)
                    .SingleAsync(m => m.Id == movieId);
                if (movie.Title != "Isolated verification edited" || movie.DurationMinutes != 95 || movie.SensoryContent.Tracks.Count != 1)
                    throw new InvalidOperationException("Movie and sensory configuration persistence failed.");
                var show = await db.Set<Show>().SingleAsync(s => s.Id == showId);
                if (show.TimeSlot.StartDateTime != localTime || show.TimeSlot.StartDateTime.Offset != TimeSpan.Zero)
                    throw new InvalidOperationException("Show time-zone conversion failed.");
                var storage = new PostgresSensoryFileStorage(db);
                await using (var stream = await storage.OpenReadAsync(path))
                {
                    using var buffer = new MemoryStream();
                    await stream.CopyToAsync(buffer);
                    if (!buffer.ToArray().SequenceEqual(content)) throw new InvalidOperationException("File contents differ.");
                }
                await storage.DeleteAsync(path);
                if (await db.Set<StoredSensoryBlob>().AnyAsync()) throw new InvalidOperationException("File deletion failed.");
            }
            logger.LogInformation("PostgreSQL isolated smoke check passed: migrations, create/edit/read, owned entities, UTC scheduling, file upload/read/delete.");
        }
        finally
        {
            // The only schema deleted is the unique one created by this invocation.
            await using var drop = new NpgsqlCommand($"DROP SCHEMA {quotedSchema} CASCADE", connection);
            await drop.ExecuteNonQueryAsync();
        }
    }
}
