using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Enums;
using Movies.Api.Mapping;
using Movies.Application.Interfaces;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Movies;

public static class CreateMovieEndpoint
{
    public const string Name = "CreateMovie";

    public static IEndpointRouteBuilder MapCreateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Movies.Create, async (
            [FromBody] CreateMovieRequest request,
            IMovieService movieService,
            IOutputCacheStore outputCacheStore,
            CancellationToken cancellationToken) =>
        {
            var movie = request.MapToMovie();
            await movieService.Create(movie, cancellationToken);
            await outputCacheStore.EvictByTagAsync(CacheTagsEnum.MovieCacheTag, cancellationToken);
            return TypedResults.CreatedAtRoute(movie.MapToResponse(), GetMovieEndpoint.Name, new
            {
                idOrSlug = movie.Id
            });
        })
            .WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status201Created)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthConstants.TrustedMemberPolicyName)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0);

        return app;
    }
}