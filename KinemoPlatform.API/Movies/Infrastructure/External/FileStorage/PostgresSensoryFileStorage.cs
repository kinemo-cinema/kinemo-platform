using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Movies.Infrastructure.External.FileStorage;

// Render Free's filesystem is ephemeral; uploaded files live with the cloud database.
public class PostgresSensoryFileStorage(PostgresAppDbContext db) : ISensoryFileStorage
{
    public async Task<string> UploadAsync(Stream content, string fileName, int movieId,
        CancellationToken cancellationToken = default)
    {
        var safeName = Path.GetFileName(fileName);
        if (string.IsNullOrWhiteSpace(safeName)) throw new ArgumentException("File name is required.", nameof(fileName));
        var path = $"movie-{movieId}/{Guid.NewGuid():N}-{safeName}";
        using var buffer = new MemoryStream();
        await content.CopyToAsync(buffer, cancellationToken);
        db.Set<StoredSensoryBlob>().Add(new() { Path = path, Content = buffer.ToArray() });
        await db.SaveChangesAsync(cancellationToken);
        return path;
    }

    public async Task DeleteAsync(string filePath, CancellationToken cancellationToken = default)
    {
        await db.Set<StoredSensoryBlob>().Where(blob => blob.Path == filePath).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<Stream> OpenReadAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var data = await db.Set<StoredSensoryBlob>().AsNoTracking().Where(blob => blob.Path == filePath)
            .Select(blob => blob.Content).SingleOrDefaultAsync(cancellationToken);
        return data is null ? throw new FileNotFoundException("Sensory file not found.", filePath) : new MemoryStream(data, false);
    }
}
