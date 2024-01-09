using Movies.Application.Models;

namespace Movies.Application.Interfaces;

public interface IMovieRepository
{
    Task<bool> Create(Movie movie, CancellationToken token = default);
    Task<Movie?> GetById(Guid id, Guid? userId = default, CancellationToken token = default);
    Task<Movie?> GetBySlug(string slug, Guid? userId = default, CancellationToken token = default);
    Task<IEnumerable<Movie>> GetAll(GetAllMoviesOptions options, CancellationToken token = default);
    Task<bool> Update(Movie movie, CancellationToken token = default);
    Task<bool> DeleteById(Guid id, CancellationToken token = default);
    Task<bool> ExistsById(Guid id, CancellationToken token = default);
}