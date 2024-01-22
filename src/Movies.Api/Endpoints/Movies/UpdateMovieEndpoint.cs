using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Enums;
using Movies.Api.Mapping;
using Movies.Application.Interfaces;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public const string Name = "UpdateMovie";

    public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Movies.Update, async (
            Guid id,
            UpdateMovieRequest request,
            IMovieService movieService,
            HttpContext context,
            IOutputCacheStore outputCacheStore,
            CancellationToken cancellationToken) =>
        {
            var movie = request.MapToMovie(id);
            var userId = context.GetUserId();
            var updatedMovie = await movieService.Update(movie, userId, cancellationToken);
            if (updatedMovie is null)
            {
                return Results.NotFound();
            }

            await outputCacheStore.EvictByTagAsync(CacheTagsEnum.MovieCacheTag, cancellationToken);
            var response = updatedMovie.MapToResponse();
            return TypedResults.Ok(response);
        })
            .WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status201Created)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthConstants.TrustedMemberPolicyName)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0);

        return app;
    }
}