using FluentValidation;
using FluentValidation.Results;
using Movies.Application.Interfaces;

namespace Movies.Application.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IMovieRepository _movieRepository;

    public RatingService(IRatingRepository ratingRepository, IMovieRepository movieRepository)
    {
        _ratingRepository = ratingRepository;
        _movieRepository = movieRepository;
    }

    public async Task<bool> RateMovie(Guid movieId, int rating, Guid userId, CancellationToken token = default)
    {
        if (rating is <= 0 or > 5)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure
                {
                    PropertyName = "rating",
                    ErrorMessage = "Rating must be between 1 and 5"
                }
            });
        }

        var movieExists = await _movieRepository.ExistsById(movieId, token);

        if (!movieExists)
        {
            return false;
        }

        return await _ratingRepository.RateMovie(movieId, rating, userId, token);
    }

    public async Task<bool> DeleteRating(Guid movieId, Guid userId, CancellationToken token = default)
    {
        return await _ratingRepository.DeleteRating(movieId, userId, token);
    }
}