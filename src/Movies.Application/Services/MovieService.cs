using Movies.Application.Interfaces;
using Movies.Application.Models;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }


    public Task<bool> Create(Movie movie)
    {
        return _movieRepository.Create(movie);
    }

    public Task<Movie?> GetById(Guid id)
    {
        return _movieRepository.GetById(id);
    }

    public Task<Movie?> GetBySlug(string slug)
    {
        return _movieRepository.GetBySlug(slug);
    }

    public Task<IEnumerable<Movie>> GetAll()
    {
        return _movieRepository.GetAll();
    }

    public async Task<Movie?> Update(Movie movie)
    {
        var movieExists = await _movieRepository.ExistsById(movie.Id);

        if (!movieExists)
        {
            return null;
        }

        await _movieRepository.Update(movie);

        return movie;
    }

    public Task<bool> DeleteById(Guid id)
    {
        return _movieRepository.DeleteById(id);
    }
}