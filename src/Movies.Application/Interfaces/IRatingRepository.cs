namespace Movies.Application.Interfaces;

public interface IRatingRepository
{
    Task<float?> GetRating(Guid movieId, CancellationToken token = default);
    Task<(float? Rating, int? UserRating)> GetRating(Guid movieId, Guid userId, CancellationToken token = default);
}