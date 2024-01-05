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


    public async Task<bool> Create(Movie movie, CancellationToken token = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, token);
        return await _movieRepository.Create(movie, token);
    }

    public Task<Movie?> GetById(Guid id, CancellationToken token = default)
    {
        return _movieRepository.GetById(id, token);
    }

    public Task<Movie?> GetBySlug(string slug, CancellationToken token = default)
    {
        return _movieRepository.GetBySlug(slug, token);
    }

    public Task<IEnumerable<Movie>> GetAll(CancellationToken token = default)
    {
        return _movieRepository.GetAll(token);
    }

    public async Task<Movie?> Update(Movie movie, CancellationToken token = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, token);
        var movieExists = await _movieRepository.ExistsById(movie.Id, token);

        if (!movieExists)
        {
            return null;
        }

        await _movieRepository.Update(movie, token);

        return movie;
    }

    public Task<bool> DeleteById(Guid id, CancellationToken token = default)
    {
        return _movieRepository.DeleteById(id, token);
    }
}