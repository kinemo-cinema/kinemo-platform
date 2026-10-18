namespace KinemoPlatform.API.Movies.Infrastructure.External.FileStorage;

/// <summary>
///     Represents the port for sensory file storage in the Kinemo Platform.
/// </summary>
/// <remarks>
///     The Kinemo Platform stores sensory track files (motion, wind, water,
///     vibration timelines) in an external storage system, not in the
///     relational database. This port abstracts the concrete provider so
///     the application layer never depends on Azure Blob, S3, or any
///     specific implementation.
/// </remarks>
public interface ISensoryFileStorage
{
    /// <summary>
    ///     Upload a sensory file to the external storage.
    /// </summary>
    /// <param name="content">
    ///     The file content stream.
    /// </param>
    /// <param name="fileName">
    ///     The original file name (used to derive the extension).
    /// </param>
    /// <param name="movieId">
    ///     The id of the movie the file belongs to. Used to scope the
    ///     storage path.
    /// </param>
    /// <param name="cancellationToken">
    ///     Optional cancellation token.
    /// </param>
    /// <returns>
    ///     The relative path or URI of the stored file, to be persisted
    ///     in the <c>file_path</c> column of the sensory track.
    /// </returns>
    Task<string> UploadAsync(
        Stream content,
        string fileName,
        int movieId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete a previously stored sensory file.
    /// </summary>
    /// <param name="filePath">
    ///     The path returned by a previous call to <see cref="UploadAsync" />.
    /// </param>
    /// <param name="cancellationToken">
    ///     Optional cancellation token.
    /// </param>
    Task DeleteAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Open a previously stored sensory file for reading.
    /// </summary>
    /// <param name="filePath">
    ///     The path returned by a previous call to <see cref="UploadAsync" />.
    /// </param>
    /// <param name="cancellationToken">
    ///     Optional cancellation token.
    /// </param>
    /// <returns>
    ///     A readable stream with the file content.
    /// </returns>
    Task<Stream> OpenReadAsync(string filePath, CancellationToken cancellationToken = default);
}