using Movies.Application.Models;

namespace Movies.Application.Interfaces;

public interface IMovieService
{
    Task<bool> Create(Movie movie);
    Task<Movie?> GetById(Guid id);
    Task<Movie?> GetBySlug(string slug);
    Task<IEnumerable<Movie>> GetAll();
    Task<Movie?> Update(Movie movie);
    Task<bool> DeleteById(Guid id);
}