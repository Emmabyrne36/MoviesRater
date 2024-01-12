using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Interfaces;
using Movies.Contracts.Requests.V1;

namespace Movies.Api.Controllers.V1;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet(ApiEndpoints.V1.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieService.GetById(id, userId, token)
            : await _movieService.GetBySlug(idOrSlug, userId, token);

        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie.MapToResponse());
    }

    [HttpGet(ApiEndpoints.V1.Movies.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllMoviesRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var options = request.MapToOptions()
            .WithUser(userId);

        var movies = await _movieService.GetAll(options, token);
        var moviesCount = await _movieService.GetCount(options.Title, options.YearOfRelease, token);

        return Ok(movies.MapToResponse(request.Page, request.PageSize, moviesCount));
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPost(ApiEndpoints.V1.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken token)
    {
        var movie = request.MapToMovie();

        await _movieService.Create(movie, token);

        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPut(ApiEndpoints.V1.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var movie = request.MapToMovie(id);
        var updatedMovie = await _movieService.Update(movie, userId, token);

        if (updatedMovie is null)
        {
            return NotFound();
        }

        return Ok(movie.MapToResponse());
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.V1.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var deleted = await _movieService.DeleteById(id, token);

        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}