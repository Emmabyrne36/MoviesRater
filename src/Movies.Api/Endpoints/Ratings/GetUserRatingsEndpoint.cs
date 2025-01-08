using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Interfaces;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Ratings;

public static class GetUserRatingsEndpoint
{
    public const string Name = "GetUserRatings";

    public static IEndpointRouteBuilder MapGetUserRatings(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Ratings.GetUserRatings, async (
            IRatingService ratingService,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var userId = context.GetUserId();
            var ratings = await ratingService.GetRatingsForUser(userId!.Value, cancellationToken);

            var ratingsResponse = ratings.MapToResponse();

            return TypedResults.Ok(ratingsResponse);
        })
            .WithName(Name)
            .Produces<IEnumerable<MovieRatingResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization()
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0);

        return app;
    }
}