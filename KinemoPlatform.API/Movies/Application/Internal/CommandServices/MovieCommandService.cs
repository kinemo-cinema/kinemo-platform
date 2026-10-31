using Cortex.Mediator;
using KinemoPlatform.API.Movies.Application.Errors;
using KinemoPlatform.API.Movies.Application.Services;
using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Movies.Domain.Model.Commands;
using KinemoPlatform.API.Movies.Domain.Model.Events;
using KinemoPlatform.API.Movies.Domain.Repositories;
using KinemoPlatform.API.Shared.Application.Patterns;
using KinemoPlatform.API.Shared.Domain.Repositories;

namespace KinemoPlatform.API.Movies.Application.Internal.CommandServices;

/// <summary>
///     Represents the movie command service in the Kinemo Platform.
/// </summary>
/// <remarks>
///     Expected failures (movie not found, duplicate title) are returned as <see cref="EMovieError" />
///     inside a <see cref="Result{TValue,TError}" />. Unexpected failures (e.g. database errors) are not
///     caught here; they surface as exceptions and are turned into a ProblemDetails 500 response globally.
/// </remarks>
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
    public async Task<Result<Movie, EMovieError>> Handle(CreateMovieCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await movieRepository.ExistsByTitleAsync(command.Title, cancellationToken))
            return Fail(EMovieError.DuplicateTitle);

        var movie = new Movie(command);
        await movieRepository.AddAsync(movie, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new MovieCreatedEvent(movie.Id, movie.Title, movie.Genre), cancellationToken);

        return Ok(movie);
    }

    /// <inheritdoc />
    public async Task<Result<Movie, EMovieError>> Handle(UpdateMovieCommand command,
        CancellationToken cancellationToken = default)
    {
        var movie = await movieRepository.FindByIdAsync(command.MovieId, cancellationToken);
        if (movie is null) return Fail(EMovieError.MovieNotFound);

        // Titles are unique (database index). Only check when the title actually changes; a change of
        // letter case alone targets the same row and must not be reported as a duplicate of itself.
        var titleChanged = !string.Equals(movie.Title, command.Title, StringComparison.OrdinalIgnoreCase);
        if (titleChanged && await movieRepository.ExistsByTitleAsync(command.Title, cancellationToken))
            return Fail(EMovieError.DuplicateTitle);

        movie.UpdateDetails(command.Title, command.DurationMinutes, command.Genre);
        movieRepository.Update(movie);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Ok(movie);
    }

    /// <inheritdoc />
    public async Task<Result<Movie, EMovieError>> Handle(DeactivateMovieCommand command,
        CancellationToken cancellationToken = default)
    {
        var movie = await movieRepository.FindByIdWithSensoryContentAsync(command.MovieId, cancellationToken);
        if (movie is null) return Fail(EMovieError.MovieNotFound);

        movie.Deactivate();
        movieRepository.Update(movie);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(new MovieDeactivatedEvent(movie.Id), cancellationToken);

        return Ok(movie);
    }

    /// <inheritdoc />
    public async Task<Result<Movie, EMovieError>> Handle(AssociateSensoryFileCommand command,
        CancellationToken cancellationToken = default)
    {
        var movie = await movieRepository.FindByIdWithSensoryContentAsync(command.MovieId, cancellationToken);
        if (movie is null) return Fail(EMovieError.MovieNotFound);

        movie.AssociateSensoryFile(command.EffectType, command.FilePath, command.DurationSeconds);
        movieRepository.Update(movie);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new SensoryFileAssociatedEvent(movie.Id, command.EffectType, command.FilePath), cancellationToken);

        return Ok(movie);
    }

    /// <inheritdoc />
    public async Task<Result<Movie, EMovieError>> Handle(ConfigureEffectsProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var movie = await movieRepository.FindByIdWithSensoryContentAsync(command.MovieId, cancellationToken);
        if (movie is null) return Fail(EMovieError.MovieNotFound);

        var previousIntensity = movie.SensoryContent.IntensityLevel;

        movie.ConfigureEffectsProfile(command.IntensityLevel);
        movieRepository.Update(movie);
        await unitOfWork.CompleteAsync(cancellationToken);

        await domainEventPublisher.PublishAsync(
            new SensoryIntensityChangedEvent(movie.Id, previousIntensity, command.IntensityLevel), cancellationToken);

        return Ok(movie);
    }

    private static Result<Movie, EMovieError> Ok(Movie movie) =>
        new Result<Movie, EMovieError>.Success(movie);

    private static Result<Movie, EMovieError> Fail(EMovieError error) =>
        new Result<Movie, EMovieError>.Failure(error);
}