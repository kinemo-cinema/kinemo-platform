using Microsoft.Extensions.Configuration;

namespace KinemoPlatform.API.Movies.Infrastructure.External.FileStorage;

/// <summary>
///     Local file system implementation of <see cref="ISensoryFileStorage" />.
/// </summary>
/// <remarks>
///     Intended for development. Files are written under a configurable
///     root folder (default: <c>storage/sensory-files</c>) relative to
///     the API's working directory. In production, replace this adapter
///     with an object storage implementation (Azure Blob, AWS S3).
/// </remarks>
/// <param name="configuration">
///     The application configuration, used to resolve the storage root.
/// </param>
public class LocalSensoryFileStorage(IConfiguration configuration) : ISensoryFileStorage
{
    private const string DefaultRoot = "storage/sensory-files";
    private const string ConfigKey = "SensoryFileStorage:LocalRoot";

    private string Root => configuration[ConfigKey] ?? DefaultRoot;

    /// <inheritdoc />
    public async Task<string> UploadAsync(
        Stream content,
        string fileName,
        int movieId,
        CancellationToken cancellationToken = default)
    {
        var safeFileName = Path.GetFileName(fileName);
        if (string.IsNullOrWhiteSpace(safeFileName))
            throw new ArgumentException("File name is required.", nameof(fileName));

        var movieFolder = Path.Combine(Root, $"movie-{movieId}");
        Directory.CreateDirectory(movieFolder);

        var relativePath = Path.Combine($"movie-{movieId}", safeFileName)
            .Replace('\\', '/');
        var absolutePath = Path.Combine(Root, relativePath);

        await using (var target = File.Create(absolutePath))
        {
            await content.CopyToAsync(target, cancellationToken);
        }

        return relativePath;
    }

    /// <inheritdoc />
    public Task DeleteAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var absolutePath = Path.Combine(Root, filePath);
        if (File.Exists(absolutePath)) File.Delete(absolutePath);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<Stream> OpenReadAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var absolutePath = Path.Combine(Root, filePath);
        if (!File.Exists(absolutePath))
            throw new FileNotFoundException($"Sensory file not found at '{filePath}'.", filePath);

        Stream stream = File.OpenRead(absolutePath);
        return Task.FromResult(stream);
    }
}