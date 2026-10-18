using KinemoPlatform.API.Movies.Domain.Model.Commands;
using KinemoPlatform.API.Movies.Domain.Model.Entities;
using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;

namespace KinemoPlatform.API.Movies.Domain.Model.Aggregates;

/// <summary>
///     Movie aggregate root entity in the Kinemo Platform.
/// </summary>
/// <remarks>
///     This class represents a 4D movie together with its sensory configuration.
///     It is the single entry point for all operations related to a movie's
///     catalog data and its associated sensory content, tracks and history.
/// </remarks>
public partial class Movie
{
    /// <summary>
    ///     Default constructor for the movie entity.
    /// </summary>
    public Movie()
    {
        Title = string.Empty;
        Genre = EMovieGenre.Other;
        Status = EMovieStatus.Active;
        SensoryContent = new SensoryContent();
    }

    /// <summary>
    ///     Constructor for the movie entity used by the persistence layer.
    /// </summary>
    public Movie(string title, int durationMinutes, EMovieGenre genre) : this()
    {
        Title = title;
        DurationMinutes = durationMinutes;
        Genre = genre;
    }

    /// <summary>
    ///     Constructor for the movie entity from a create command.
    /// </summary>
    /// <param name="command">
    ///     The <see cref="CreateMovieCommand" /> command.
    /// </param>
    public Movie(CreateMovieCommand command) : this(command.Title, command.DurationMinutes, command.Genre)
    {
    }

    public int Id { get; }
    public string Title { get; private set; }
    public int DurationMinutes { get; private set; }
    public EMovieGenre Genre { get; private set; }
    public EMovieStatus Status { get; private set; }

    /// <summary>
    ///     Sensory content associated with this movie (1-to-1).
    /// </summary>
    public SensoryContent SensoryContent { get; private set; }

    /// <summary>
    ///     Update the basic information of the movie.
    /// </summary>
    /// <param name="title">The new title.</param>
    /// <param name="durationMinutes">The new duration in minutes.</param>
    /// <param name="genre">The new genre.</param>
    public void UpdateDetails(string title, int durationMinutes, EMovieGenre genre)
    {
        Title = title;
        DurationMinutes = durationMinutes;
        Genre = genre;
    }

    /// <summary>
    ///     Mark the movie as active, making it available for scheduling.
    /// </summary>
    public void Activate()
    {
        Status = EMovieStatus.Active;
    }

    /// <summary>
    ///     Mark the movie as inactive, preventing it from being scheduled
    ///     in new shows while preserving it in the catalog.
    /// </summary>
    public void Deactivate()
    {
        Status = EMovieStatus.Inactive;
    }

    /// <summary>
    ///     Associate a sensory file to this movie's sensory content.
    /// </summary>
    /// <param name="effectType">
    ///     The <see cref="EEffectType" /> channel of the track.
    /// </param>
    /// <param name="filePath">
    ///     The storage path where the sensory file was saved.
    /// </param>
    /// <param name="durationSeconds">
    ///     The duration of the track in seconds.
    /// </param>
    public void AssociateSensoryFile(EEffectType effectType, string filePath, int durationSeconds)
    {
        SensoryContent.AddTrack(effectType, filePath, durationSeconds);
    }

    /// <summary>
    ///     Configure the effects profile of this movie by changing the
    ///     sensory intensity level and recording the change in the history.
    /// </summary>
    /// <param name="newIntensity">
    ///     The new <see cref="ESensoryIntensityLevel" /> to apply.
    /// </param>
    public void ConfigureEffectsProfile(ESensoryIntensityLevel newIntensity)
    {
        SensoryContent.ChangeIntensity(newIntensity);
    }
}