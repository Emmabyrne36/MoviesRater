using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Enums;
using Movies.Application.Interfaces;

namespace Movies.Api.Endpoints.Movies;

public static class DeleteMovieEndpoint
{
    public const string Name = "DeleteMovie";

    public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Movies.Delete, async (
            Guid id,
            IMovieService movieService,
            IOutputCacheStore outputCacheStore,
            CancellationToken cancellationToken) =>
        {
            var deleted = await movieService.DeleteById(id, cancellationToken);
            if (!deleted)
            {
                return Results.NotFound();
            }

            await outputCacheStore.EvictByTagAsync(CacheTagsEnum.MovieCacheTag, cancellationToken);
            return Results.Ok();
        })
            .WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0);


        return app;
    }
}