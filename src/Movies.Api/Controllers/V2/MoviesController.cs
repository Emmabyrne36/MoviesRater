using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Interfaces;

namespace Movies.Api.Controllers.V2;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet(ApiEndpoints.V2.Movies.Get)]
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
}