using Movies.Api.Auth;
using Movies.Application.Interfaces;
using Movies.Contracts.Requests;

namespace Movies.Api.Endpoints.Ratings;

public static class CreateMovieRatingEndpoint
{
    public const string Name = "CreateRating";

    public static IEndpointRouteBuilder MapCreateRating(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Movies.Rate, async (
            Guid id,
            RateMovieRequest request,
            IRatingService ratingService,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var userId = context.GetUserId();
            var result = await ratingService.RateMovie(id, request.Rating, userId!.Value, cancellationToken);

            return result ? TypedResults.Ok() : Results.NotFound();
        })
            .WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization()
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0);

        return app;
    }
}