using Movies.Application.Models;

namespace Movies.Application.Interfaces;

public interface IMovieService
{
    Task<bool> Create(Movie movie, CancellationToken token = default);
    Task<Movie?> GetById(Guid id, CancellationToken token = default);
    Task<Movie?> GetBySlug(string slug, CancellationToken token = default);
    Task<IEnumerable<Movie>> GetAll(CancellationToken token = default);
    Task<Movie?> Update(Movie movie, CancellationToken token = default);
    Task<bool> DeleteById(Guid id, CancellationToken token = default);
}