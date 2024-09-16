using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/reviews")]
[ApiController]
public class ReviewsController(IReviewService reviewService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var res = await reviewService.GetListReviews();

        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReview([FromQuery] Guid id)
    {
        var res = await reviewService.GetListReviews();

        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewReq req)
    {
        var res = await reviewService.CreateReview(req);

        return Ok(res);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReview([FromRoute] Guid id,[FromBody] UpdateReviewReq req)
    {
        var res = await reviewService.UpdateReview(id, req);

        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
    {
        await reviewService.DeleteReview(id);

        return Ok();
    }
}
