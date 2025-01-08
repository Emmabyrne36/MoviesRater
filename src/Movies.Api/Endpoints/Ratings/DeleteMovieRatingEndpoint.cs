using Movies.Api.Auth;
using Movies.Application.Interfaces;

namespace Movies.Api.Endpoints.Ratings;

public static class DeleteMovieRatingEndpoint
{
    public const string Name = "DeleteRating";

    public static IEndpointRouteBuilder MapDeleteRating(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Movies.DeleteRating, async (
            Guid id,
            IRatingService ratingService,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var userId = context.GetUserId();
            var result = await ratingService.DeleteRating(id, userId!.Value, cancellationToken);

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