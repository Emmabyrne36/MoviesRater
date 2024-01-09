using Movies.Application.Models;

namespace Movies.Application.Interfaces;

public interface IMovieService
{
    Task<bool> Create(Movie movie, CancellationToken token = default);
    Task<Movie?> GetById(Guid id, Guid? userId = default, CancellationToken token = default);
    Task<Movie?> GetBySlug(string slug, Guid? userId = default, CancellationToken token = default);
    Task<IEnumerable<Movie>> GetAll(GetAllMoviesOptions options, CancellationToken token = default);
    Task<Movie?> Update(Movie movie, Guid? userId = default, CancellationToken token = default);
    Task<bool> DeleteById(Guid id, CancellationToken token = default);
    Task<int> GetCount(string? title, int? yearOfRelease, CancellationToken token = default);
}