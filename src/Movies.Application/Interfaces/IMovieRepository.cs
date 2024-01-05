using Movies.Application.Models;

namespace Movies.Application.Interfaces;

public interface IMovieRepository
{
    Task<bool> Create(Movie movie);
    Task<Movie?> GetById(Guid id);
    Task<Movie?> GetBySlug(string slug);
    Task<IEnumerable<Movie>> GetAll();
    Task<bool> Update(Movie movie);
    Task<bool> DeleteById(Guid id);
    Task<bool> ExistsById(Guid id);
}