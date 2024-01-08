namespace Movies.Application.Interfaces;

public interface IRatingService
{
    Task<bool> RateMovie(Guid movieId, int rating, Guid userId, CancellationToken token = default);
    Task<bool> DeleteRating(Guid movieId, Guid userId, CancellationToken token = default);

}