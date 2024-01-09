using FluentValidation;
using Movies.Application.Interfaces;
using Movies.Application.Models;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IRatingRepository _ratingRepository;
    private readonly IValidator<Movie> _movieValidator;
    private readonly IValidator<GetAllMoviesOptions> _optionsValidator;

    public MovieService(IMovieRepository movieRepository, IRatingRepository ratingRepository, IValidator<Movie> movieValidator, IValidator<GetAllMoviesOptions> optionsValidator)
    {
        _movieRepository = movieRepository;
        _ratingRepository = ratingRepository;
        _movieValidator = movieValidator;
        _optionsValidator = optionsValidator;
    }

    public Task<Movie?> GetById(Guid id, Guid? userId = default, CancellationToken token = default)
    {
        return _movieRepository.GetById(id, userId, token);
    }

    public Task<Movie?> GetBySlug(string slug, Guid? userId = default, CancellationToken token = default)
    {
        return _movieRepository.GetBySlug(slug, userId, token);
    }

    public async Task<IEnumerable<Movie>> GetAll(GetAllMoviesOptions options, CancellationToken token = default)
    {
        await _optionsValidator.ValidateAndThrowAsync(options, token);
        return await _movieRepository.GetAll(options, token);
    }


    public async Task<bool> Create(Movie movie, CancellationToken token = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, token);
        return await _movieRepository.Create(movie, token);
    }

    public async Task<Movie?> Update(Movie movie, Guid? userId = default, CancellationToken token = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, token);
        var movieExists = await _movieRepository.ExistsById(movie.Id, token);

        if (!movieExists)
        {
            return null;
        }

        await _movieRepository.Update(movie, token);

        if (!userId.HasValue)
        {
            var rating = await _ratingRepository.GetRating(movie.Id, token);
            movie.Rating = rating;
            return movie;
        }

        var ratings = await _ratingRepository.GetRating(movie.Id, userId.Value, token);
        movie.Rating = ratings.Rating;
        movie.UserRating = ratings.UserRating;

        return movie;
    }

    public Task<bool> DeleteById(Guid id, CancellationToken token = default)
    {
        return _movieRepository.DeleteById(id, token);
    }

    public Task<int> GetCount(string? title, int? yearOfRelease, CancellationToken token = default)
    {
        return _movieRepository.GetCount(title, yearOfRelease, token);
    }
}