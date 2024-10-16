using Fvent.Service.Request;
using Fvent.Service.Services;
using Fvent.Service.Services.Imp;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/events")]
[ApiController]
public class EventsController(IEventService eventService,
                              ICommentService commentService,
                              IEventFollowerService eventFollowerService,
                              IEventRegistationService eventResgistationService) : ControllerBase
{
    #region Event
    [HttpGet]
    public async Task<IActionResult> GetListEvents([FromQuery] GetEventsRequest request)
    {
        var res = await eventService.GetListEvents(request);

        return Ok(res);
    }

    [HttpGet("organizer")]
    public async Task<IActionResult> GetListEventsByOrganizer([FromQuery] IdReq organizerId)
    {
        var res = await eventService.GetListEventsByOrganizer(organizerId.Id);

        return Ok(res);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var res = await eventService.GetEvent(id);

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

    #region Event Rating                                                                              
    [HttpGet]
    [Route("{id}/average-rating")]
    public async Task<IActionResult> GetEventRate([FromRoute] Guid id)
    {
        var res = await eventService.GetEventRate(new IdReq(id));

        return Ok(res);
    }
    #endregion

    #region Event Following
    [HttpPost("{id}/follow")]
    public async Task<IActionResult> FollowEvent(Guid id, [FromBody] IdReq userId)
    {
        var res = await eventFollowerService.FollowEvent(id, userId.Id);

        return Ok(res);
    }

    [HttpDelete("{id}/unfollow")]
    public async Task<IActionResult> UnfollowEvent(Guid id, [FromBody] IdReq userId)
    {
        await eventFollowerService.UnfollowEvent(id, userId.Id);

        return Ok();
    }
    #endregion

    #region Event Registration
    [HttpPost("{id}/register")]
    public async Task<IActionResult> RegisterEvent(Guid id, [FromBody] IdReq userId)
    {
        var res = await eventResgistationService.RegisterFreeEvent(id, userId.Id);

        return Ok(res);
    }

    [HttpDelete("{id}/unregister")]
    public async Task<IActionResult> UnRegisterEvent(Guid id, [FromBody] IdReq userId)
    {
        await eventResgistationService.UnRegisterEvent(id, userId.Id);

        return Ok();
    }

    [HttpGet("{eventId}/participants")]
    public async Task<IActionResult> GetParticipantsForEvent([FromRoute] Guid eventId)
    {
        var res = await eventResgistationService.GetAllParticipantsForEvent(eventId);
        return Ok(res);
    }

    #endregion

    #region Event Reviewing
    [HttpPost]
    [Route("{id}/reviews")]
    public async Task<IActionResult> CreateReview([FromRoute] Guid id, [FromBody] CreateReviewReq req)
    {
        var res = await eventService.CreateReview(new CreateReviewReq(req.Rating, req.Comment, id, req.UserId));

        return Ok(res);
    }

    [HttpGet]
    [Route("{id}/reviews")]
    public async Task<IActionResult> GetEventReviews([FromRoute] Guid id)
    {
        var res = await eventService.GetEventReviews(new IdReq(id));

        return Ok(res);
    }
    #endregion

    #region Event User
    [HttpGet]
    [Route("api/events/{id}/paticipants")]
    public async Task<IActionResult> GetEventRegisters([FromRoute] Guid id)
    {
        var res = await eventService.GetEventRegisters(new IdReq(id));

        return Ok(res);
    }

    [HttpGet("{eventId}/participants")]
    public async Task<IActionResult> GetParticipantsForEvent([FromRoute] Guid eventId)
    {
        var res = await eventResgistationService.GetAllParticipantsForEvent(eventId);

        return Ok(res);
    }
    #endregion

    #region Event-Comment
    [HttpGet("{id}/comments")]
    public async Task<IActionResult> GetComments(Guid id)
    {
        var res = await commentService.GetListComments(id);

        return Ok(res);
    }

    [HttpPost("{id}/comments")]
    public async Task<IActionResult> CreateComment(Guid id, [FromBody] CreateCommentReq req)
    {
        var res = await commentService.CreateComment(id, req);

        return Ok(res);
    }
    #endregion
}
