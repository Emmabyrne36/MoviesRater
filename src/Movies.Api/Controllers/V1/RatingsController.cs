using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Interfaces;
using Movies.Contracts.Requests.V1;

namespace Movies.Api.Controllers.V1;

[ApiController]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [Authorize]
    [HttpGet(ApiEndpoints.V1.Ratings.GetUserRatings)]
    public async Task<IActionResult> GetUserRatings(CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var ratings = await _ratingService.GetRatingsForUser(userId.Value, token);

        var ratingsResponse = ratings.MapToResponse();

        return Ok(ratingsResponse);
    }

    [Authorize]
    [HttpPut(ApiEndpoints.V1.Movies.Rate)]
    public async Task<IActionResult> RateMovie([FromRoute] Guid id, [FromBody] RateMovieRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var result = await _ratingService.RateMovie(id, request.Rating, userId.Value, token);

        return result ? Ok() : NotFound();
    }

    [Authorize]
    [HttpDelete(ApiEndpoints.V1.Movies.DeleteRating)]
    public async Task<IActionResult> DeleteRating([FromRoute] Guid id, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var result = await _ratingService.DeleteRating(id, userId.Value, token);

        return result ? Ok() : NotFound();
    }
}