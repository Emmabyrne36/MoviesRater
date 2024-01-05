using FluentValidation;
using Movies.Application.Interfaces;
using Movies.Application.Models;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<Movie> _movieValidator;

    public MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator)
    {
        _movieRepository = movieRepository;
        _movieValidator = movieValidator;
    }


    public async Task<bool> Create(Movie movie)
    {
        await _movieValidator.ValidateAndThrowAsync(movie);
        return await _movieRepository.Create(movie);
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
        await _movieValidator.ValidateAndThrowAsync(movie);
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