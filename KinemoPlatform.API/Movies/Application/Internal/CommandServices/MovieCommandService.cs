using Cortex.Mediator;
using KinemoPlatform.API.Movies.Application.Services;
using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Movies.Domain.Model.Commands;
using KinemoPlatform.API.Movies.Domain.Model.Events;
using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;
using KinemoPlatform.API.Movies.Domain.Repositories;
using KinemoPlatform.API.Movies.Domain.Services;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Movies.Application.Internal.CommandServices;

/// <summary>
///     Represents the movie command service in the Kinemo Platform.
/// </summary>
/// <param name="movieRepository">
///     The <see cref="IMovieRepository" /> to use.
/// </param>
/// <param name="unitOfWork">
///     The <see cref="IUnitOfWork" /> to use.
/// </param>
/// <param name="domainEventPublisher">
///     The <see cref="IMediator" /> to use for publishing domain events.
/// </param>
public class MovieCommandService(
    IMovieRepository movieRepository,
    IUnitOfWork unitOfWork,
    IMediator domainEventPublisher) : IMovieCommandService
{
    /// <inheritdoc />
    public async Task<Movie?> Handle(CreateMovieCommand command)
    {
        if (await movieRepository.ExistsByTitleAsync(command.Title))
            throw new InvalidOperationException($"A movie with title '{command.Title}' already exists.");

        var movie = new Movie(command);
        await movieRepository.AddAsync(movie);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new MovieCreatedEvent(movie.Id, movie.Title, movie.Genre));

        return movie;
    }

    /// <inheritdoc />
    public async Task<Movie?> Handle(UpdateMovieCommand command)
    {
        var movie = await movieRepository.FindByIdAsync(command.MovieId);
        if (movie is null) throw new InvalidOperationException($"Movie with id {command.MovieId} was not found.");

        movie.UpdateDetails(command.Title, command.DurationMinutes, command.Genre);
        movieRepository.Update(movie);
        await unitOfWork.CompleteAsync();

        return movie;
    }

    /// <inheritdoc />
    public async Task<Movie?> Handle(DeactivateMovieCommand command)
    {
        var movie = await movieRepository.FindByIdWithSensoryContentAsync(command.MovieId);
        if (movie is null) throw new InvalidOperationException($"Movie with id {command.MovieId} was not found.");

        movie.Deactivate();
        movieRepository.Update(movie);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(new MovieDeactivatedEvent(movie.Id));

        return movie;
    }

    /// <inheritdoc />
    public async Task<Movie?> Handle(AssociateSensoryFileCommand command)
    {
        var movie = await movieRepository.FindByIdWithSensoryContentAsync(command.MovieId);
        if (movie is null) throw new InvalidOperationException($"Movie with id {command.MovieId} was not found.");

        movie.AssociateSensoryFile(command.EffectType, command.FilePath, command.DurationSeconds);
        movieRepository.Update(movie);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new SensoryFileAssociatedEvent(movie.Id, command.EffectType, command.FilePath));

        return movie;
    }

    /// <inheritdoc />
    public async Task<Movie?> Handle(ConfigureEffectsProfileCommand command)
    {
        var movie = await movieRepository.FindByIdWithSensoryContentAsync(command.MovieId);
        if (movie is null) throw new InvalidOperationException($"Movie with id {command.MovieId} was not found.");

        var previousIntensity = movie.SensoryContent.IntensityLevel;

        movie.ConfigureEffectsProfile(command.IntensityLevel);
        movieRepository.Update(movie);
        await unitOfWork.CompleteAsync();

        await domainEventPublisher.PublishAsync(
            new SensoryIntensityChangedEvent(movie.Id, previousIntensity, command.IntensityLevel));

        return movie;
    }
}