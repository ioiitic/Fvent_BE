using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fvent.API.Controllers;

[Route("api/events")]
[ApiController]
public class EventsController(IEventService eventService,
                              ICommentService commentService,
                              IFollowerService followerService,
                              IRatingService ratingService,
                              IRegistationService resgistationService,
                              IReviewService reviewService,
                              IUserService userService) : ControllerBase
{
    #region Event
    /// <summary>
    /// Get list events
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetListEvents([FromQuery] GetEventsRequest request)
    {
        var res = await eventService.GetListEvents(request);

        return Ok(res);
    }

    /// <summary>
    /// Get an event detail
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpGet("{eventId}")]
    public async Task<IActionResult> GetEvent(Guid eventId)
    {
        var res = await eventService.GetEvent(eventId);

        return Ok(res);
    }

    /// <summary>
    /// Get list events belong to organizer
    /// </summary>
    /// <param name="organizerId"></param>
    /// <returns></returns>
    [HttpGet("organizer")]
    public async Task<IActionResult> GetListEventsByOrganizer([FromQuery] IdReq organizerId)
    {
        var res = await eventService.GetListEventsByOrganizer(organizerId.Id);

        return Ok(res);
    }

    /// <summary>
    /// Get list events recommended for user
    /// </summary>
    /// <returns></returns>
    [HttpGet("recommendation")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetListRecommend()
    {
        var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var user = await userService.GetByEmail(email!);

        var res = await eventService.GetListRecommend(new IdReq(user.UserId));

        return Ok(res);
    }

    /// <summary>
    /// Create an event
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventReq req)
    {
        var res = await eventService.CreateEvent(req);

        return Ok(res);
    }

    /// <summary>
    /// Update event info
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{eventId}")]
    public async Task<IActionResult> UpdateEvent([FromRoute] Guid eventId, [FromBody] UpdateEventReq req)
    {
        var res = await eventService.UpdateEvent(eventId, req);

        return Ok(res);
    }

    /// <summary>
    /// Soft delete an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpDelete("{eventId}")]
    public async Task<IActionResult> DeleteEvent([FromRoute] Guid eventId)
    {
        await eventService.DeleteEvent(eventId);

        return Ok();
    }
    #endregion

    #region Event Comment
    /// <summary>
    /// Get comments of an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpGet("{eventId}/comments")]
    public async Task<IActionResult> GetComments(Guid eventId)
    {
        var res = await commentService.GetListComments(eventId);

        return Ok(res);
    }

    /// <summary>
    /// Comment on an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("{eventId}/comments")]
    public async Task<IActionResult> CreateComment(Guid eventId, [FromBody] CreateCommentReq req)
    {
        var res = await commentService.CreateComment(eventId, req);

        return Ok(res);
    }
    #endregion

    #region Event Following
    /// <summary>
    /// Follow an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPost("{eventId}/follow")]
    public async Task<IActionResult> FollowEvent(Guid eventId, [FromBody] IdReq userId)
    {
        var res = await followerService.FollowEvent(eventId, userId.Id);

        return Ok(res);
    }

    /// <summary>
    /// Unfollow an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{eventId}/unfollow")]
    public async Task<IActionResult> UnfollowEvent(Guid eventId, [FromBody] IdReq userId)
    {
        await followerService.UnfollowEvent(eventId, userId.Id);

        return Ok();
    }
    #endregion

    #region Event Rating
    /// <summary>
    /// Get averate rating of an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpGet("{eventId}/average-rating")]
    public async Task<IActionResult> GetEventRate([FromRoute] Guid eventId)
    {
        var res = await ratingService.GetEventRate(eventId);

        return Ok(res);
    }
    #endregion

    #region Event Registration
    /// <summary>
    /// Register an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPost("{eventId}/register")]
    public async Task<IActionResult> RegisterEvent(Guid eventId, [FromBody] IdReq userId)
    {
        var res = await resgistationService.RegisterFreeEvent(eventId, userId.Id);

        return Ok(res);
    }

    /// <summary>
    /// Unregister an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{eventId}/unregister")]
    public async Task<IActionResult> UnRegisterEvent(Guid eventId, [FromBody] IdReq userId)
    {
        await resgistationService.UnRegisterEvent(eventId, userId.Id);

        return Ok();
    }

    // TODO: Them field get theo thang, nam
    /// <summary>
    /// Get all events that register
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("participant")]
    public async Task<IActionResult> GetEventRegisters()
    {
        var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var user = await userService.GetByEmail(email!);

        var res = await eventService.GetEventRegisters(user.UserId);

        return Ok(res);
    }
    #endregion

    #region Event Reviewing
    /// <summary>
    /// GET api/events/{eventId}/reviews
    /// Get a list of event reviews
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{eventId}/reviews")]
    public async Task<IActionResult> GetEventReviews([FromRoute] Guid eventId)
    {
        var res = await reviewService.GetReview(eventId);

        return Ok(res);
    }

    /// <summary>
    /// POST api/events/{eventId}/reviews
    /// Review an event
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("{eventId}/reviews")]
    public async Task<IActionResult> CreateReview([FromRoute] Guid eventId, [FromBody] CreateReviewReq req)
    {
        var res = await reviewService.CreateReview(eventId, req);

        return Ok(res);
    }
    #endregion

    /// <summary>
    /// Get all user 
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    //[HttpGet("{eventId}/participants")]
    //public async Task<IActionResult> GetParticipantsForEvent([FromRoute] Guid eventId)
    //{
    //    var res = await registationService.GetAllParticipantsForEvent(eventId);

    //    return Ok(res);
    //}
}
