using Fvent.Service.Request;
using Fvent.Service.Services;
using Fvent.Service.Services.Imp;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/events")]
[ApiController]
public class EventsController(IEventService eventService) : ControllerBase
{
    #region CRUD Event
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var res = await eventService.GetListEvents();

        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent([FromQuery] Guid id)
    {
        var res = await eventService.GetListEvents();

        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventReq req)
    {
        var res = await eventService.CreateEvent(req);

        return Ok(res);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent([FromRoute] Guid id, [FromBody] UpdateEventReq req)
    {
        var res = await eventService.UpdateEvent(id, req);

        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent([FromRoute] Guid id)
    {
        await eventService.DeleteEvent(id);

        return Ok();
    }
    #endregion

    #region Event
    [HttpGet]
    [Route("api/events/{id}/average-rating")]
    public async Task<IActionResult> GetEventRate([FromRoute] Guid id)
    {
        var res = await eventService.GetEventRate(new IdReq(id));

        return Ok(res);
    }
    #endregion

    #region Event-Review
    [HttpPost]
    [Route("api/events/{id}/reviews")]
    public async Task<IActionResult> CreateReview([FromRoute] Guid id, [FromBody] CreateReviewReq req)
    {
        var res = await eventService.CreateReview(new CreateReviewReq(req.Rating, req.Comment, id, req.UserId));

        return Ok(res);
    }

    [HttpGet]
    [Route("api/events/{id}/reviews")]
    public async Task<IActionResult> GetEventReviews([FromRoute] Guid id)
    {
        var res = await eventService.GetEventReviews(new IdReq(id));

        return Ok(res);
    }
    #endregion

    #region Event-User
    [HttpGet]
    [Route("api/events/{id}/paticipants")]
    public async Task<IActionResult> GetEventRegisters([FromRoute] Guid id)
    {
        var res = await eventService.GetEventRegisters(new IdReq(id));

        return Ok(res);
    }
    #endregion
}
