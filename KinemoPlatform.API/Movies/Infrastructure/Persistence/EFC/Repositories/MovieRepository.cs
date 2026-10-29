using KinemoPlatform.API.Movies.Domain.Model.Aggregates;
using KinemoPlatform.API.Movies.Domain.Model.ValueObjects;
using KinemoPlatform.API.Movies.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KinemoPlatform.API.Movies.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
///     Represents the movie repository implementation in the Kinemo Platform.
/// </summary>
/// <param name="context">
///     The <see cref="AppDbContext" /> to use.
/// </param>
public class MovieRepository(AppDbContext context)
    : BaseRepository<Movie>(context), IMovieRepository
{
    /// <inheritdoc />
    public async Task<bool> ExistsByTitleAsync(string title)
    {
        return await Context.Set<Movie>().AnyAsync(m => m.Title == title);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Movie>> FindByTitleOrGenreAsync(string? title, EMovieGenre? genre)
    {
        var query = Context.Set<Movie>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(m => m.Title.Contains(title));

        if (genre.HasValue)
            query = query.Where(m => m.Genre == genre.Value);

        return await query.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Movie?> FindByIdWithSensoryContentAsync(int id)
    {
        return await Context.Set<Movie>()
            .Include(m => m.SensoryContent)
                .ThenInclude(sc => sc.Tracks)
            .Include(m => m.SensoryContent)
                .ThenInclude(sc => sc.History)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    // <inheritdoc />
    public override async Task<Movie?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Movie>()
            .Include(m => m.SensoryContent)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    // <inheritdoc />
    public override async Task<IEnumerable<Movie>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Movie>()
            .Include(m => m.SensoryContent)
            .ToListAsync(cancellationToken);
    }
}