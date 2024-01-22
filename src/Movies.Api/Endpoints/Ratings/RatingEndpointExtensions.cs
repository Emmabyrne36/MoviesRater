namespace Movies.Api.Endpoints.Ratings;

public static class RatingEndpointExtensions
{
    public static IEndpointRouteBuilder MapRatingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreateRating();
        app.MapGetUserRatings();
        app.MapDeleteRating();
        return app;
    }
}
