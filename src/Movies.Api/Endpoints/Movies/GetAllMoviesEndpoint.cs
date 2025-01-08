using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Interfaces;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Movies;

public static class GetMoviesEndpoint
{
    public const string Name = "GeAllMovies";

    public static IEndpointRouteBuilder MapGetAllMovies(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Movies.GetAll, async (
                [AsParameters] GetAllMoviesRequest request, IMovieService movieService,
                HttpContext context, CancellationToken cancellationToken) =>
        {
            var userId = context.GetUserId();
            var options = request.MapToOptions()
                .WithUser(userId);
            var movies = await movieService.GetAll(options, cancellationToken);
            var movieCount = await movieService.GetCount(options.Title, options.YearOfRelease, cancellationToken);
            var moviesResponse = movies.MapToResponse(
                request.Page.GetValueOrDefault(PagedRequest.DefaultPage),
                request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize),
                movieCount);
            return TypedResults.Ok(moviesResponse);
        })
            .WithName($"{Name}V1")
            .Produces<MoviesResponse>(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0);

        app.MapGet(ApiEndpoints.Movies.GetAll, async (
                [AsParameters] GetAllMoviesRequest request, IMovieService movieService,
                HttpContext context, CancellationToken token) =>
        {
            var userId = context.GetUserId();
            var options = request.MapToOptions()
                .WithUser(userId);
            var movies = await movieService.GetAll(options, token);
            var movieCount = await movieService.GetCount(options.Title, options.YearOfRelease, token);
            var moviesResponse = movies.MapToResponse(
                request.Page.GetValueOrDefault(PagedRequest.DefaultPage),
                request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize),
                movieCount);
            return TypedResults.Ok(moviesResponse);
        })
            .WithName($"{Name}V2")
            .Produces<MoviesResponse>(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(2.0)
            .CacheOutput("MovieCache");
        return app;
    }
}