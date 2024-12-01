using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Service.Request;
using Fvent.Service.Services;
using Fvent.Service.Services.Imp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fvent.API.Controllers;

[Route("api/events")]
[ApiController]
public class EventsController(IEventService eventService, IRatingService ratingService,
                              IRegistationService registationService, IReviewService reviewService,
                              IFormService formService) : ControllerBase
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
    /// Get list events
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("getAllEvents")]
    [Authorize(Roles = "admin, moderator")]
    public async Task<IActionResult> GetListEventsForAdmin([FromQuery] GetEventsRequest request)
    {
        var res = await eventService.GetListEventsForAdmin(request);

        return Ok(res);
    }

    /// <summary>
    /// Get list event's banners
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("banners")]
    public async Task<IActionResult> GetListEventBanners()
    {
        var res = await eventService.GetEventBanners();

        return Ok(res);
    }

    /// <summary>
    /// Get an event detail
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpGet("{eventId}")]
    public async Task<IActionResult> GetEvent([FromRoute] Guid eventId)
    {
        Guid? userId = null;
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var parsedUserId))
        {
            userId = parsedUserId;
        }

        var res = await eventService.GetEvent(eventId, userId);

        return Ok(res);
    }


    /// <summary>
    /// Get list events belong to organizer
    /// </summary>
    /// <param name="organizerId"></param>
    /// <returns></returns>
    [HttpGet("organizerPublic")]
    public async Task<IActionResult> GetListEventsByOrganizer([FromQuery] GetEventByOrganizerReq req)
    {
        var res = await eventService.GetListEventsByOrganizer(req);

        return Ok(res);
    }

    [HttpGet("organizerPrivate")]
    public async Task<IActionResult> GetListEventsOfOrganizer([FromQuery] GetEventOfOrganizerReq req)
    {
        var res = await eventService.GetListEventsOfOrganizer(req);

        return Ok(res);
    }

    /// <summary>
    /// Get list events recommended for user
    /// </summary>
    /// <returns></returns>
    [HttpGet("recommendation")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> GetListRecommend()
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }

        var res = await eventService.GetListRecommend(userId);

        return Ok(res);
    }

    /// <summary>
    /// Create an event
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "organizer")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventReq req)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var organizerId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }

        var res = await eventService.CreateEvent(req, organizerId);

        return Ok(res);
    }

    /// <summary>
    /// Update event info
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{eventId}")]
    [Authorize(Roles = "organizer")]
    public async Task<IActionResult> UpdateEvent([FromRoute] Guid eventId, [FromBody] UpdateEventReq req)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var organizerId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }

        var res = await eventService.UpdateEvent(eventId, organizerId, req);

        return Ok(res);
    }

    /// <summary>
    /// Organizer publish event for review
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpPut("{eventId}/submit")]
    [Authorize(Roles = "organizer")]
    public async Task<IActionResult> SubmitEvent([FromRoute] Guid eventId)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var organizerId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }
        var res = await eventService.SubmitEvent(eventId, organizerId);

        return Ok(res);
    }

    /// <summary>
    /// Moderator approve Event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="isApproved"></param>
    /// <param name="processNote"></param>
    /// <returns></returns>
    [HttpPut("{eventId}/approve")]
    [Authorize(Roles = "moderator")]
    public async Task<IActionResult> ApproveEvent([FromRoute] Guid eventId, [FromQuery] bool isApproved, [FromBody] ApproveEventRequest processNote)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }

        var res = await eventService.ApproveEvent(eventId, isApproved, userId, processNote.ProcessNote);

        return Ok(res);
    }

    /// <summary>
    /// Use for registerd user check-in an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpPut("{eventId}/checkin")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> CheckinEvent([FromRoute] Guid eventId)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }

        try
        {
            await eventService.CheckinEvent(eventId, userId, false);
            return Ok("Check-in successful");
        }
        catch (NotFoundException)
        {
            return NotFound("Please register for the event first.");
        }
    }

    [HttpPut("{eventId}/checkin-organizer")]
    [Authorize(Roles = "organizer")]
    public async Task<IActionResult> CheckinEventByOrganizer([FromRoute] Guid eventId, [FromQuery] Guid userId)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            await eventService.CheckinEvent(eventId, userId, true);
            return Ok("Check-in successful");
        }
        catch (NotFoundException)
        {
            return NotFound("Please register for the event first.");
        }
    }

    /// <summary>
    /// Soft delete an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpDelete("{eventId}")]
    [Authorize(Roles = "organizer")]
    public async Task<IActionResult> DeleteEvent([FromRoute] Guid eventId)
    {
        await eventService.DeleteEvent(eventId);

        return Ok();
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
    [Authorize(Roles = "student")]
    public async Task<IActionResult> RegisterEvent(Guid eventId)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }

        var res = await registationService.RegisterFreeEvent(eventId, userId);

        return Ok(res);
    }

    /// <summary>
    /// Unregister an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{eventId}/unregister")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> UnRegisterEvent(Guid eventId)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }

        await registationService.UnRegisterEvent(eventId, userId);

        return Ok();
    }

    /// <summary>
    /// Get all users registered an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpGet("{eventId}/participants")]
    [Authorize(Roles = "organizer")]
    public async Task<IActionResult> GetParticipantsForEvent([FromRoute] Guid eventId, [FromQuery] GetRegisteredUsersReq req)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }

        var res = await eventService.GetRegisteredUsers(eventId, req, userId);

        return Ok(res);
    }
    #endregion

    #region Event Reviewing
    /// <summary>
    /// GET api/events/{eventId}/reviews
    /// Get a list of event reviews
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{eventId}/reviews")]
    public async Task<IActionResult> GetEventReviews([FromRoute] Guid eventId)
    {
        var res = await reviewService.GetListReviews(eventId);

        return Ok(res);
    }

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

    /// <summary>
    /// POST api/events/{eventId}/reviews
    /// Review an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("{eventId}/reviews")]
    public async Task<IActionResult> CreateReview([FromRoute] Guid eventId, [FromBody] CreateReviewReq req)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }
        var res = await reviewService.CreateReview(eventId, userId, req);

        return Ok(res);
    }
    #endregion

    #region Event Form
    /// <summary>
    /// Get form submits for an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpGet("{eventId}/form-submit")]

    public async Task<IActionResult> GetFormSubmits([FromRoute] Guid eventId)
    {
        var res = await formService.GetFormSubmits(eventId);

        return Ok(res);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("{eventId}/submit-form")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> SubmitForm([FromRoute] Guid eventId, FormSubmitReq req)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }

        var res = await formService.SubmitForm(eventId, userId, req);

        return Ok(res);
    }

    [HttpGet("{eventId}/get-user-formSubmit")]
    public async Task<IActionResult> UserFormSubmit([FromRoute] Guid eventId, [FromQuery] Guid userId)
    {

        var res = await formService.GetFormSubmit(eventId, userId);

        return Ok(res);
    }
    #endregion

    #region Report
    [HttpGet("report")]
    public async Task<IActionResult> Report()
    {
        var res = await eventService.Report();

        return Ok(res);
    }

    [HttpGet("{eventId}/report")]
    public async Task<IActionResult> ReportByEvent(Guid eventId)
    {
        var res = await eventService.ReportByEvent(eventId);

        return Ok(res);
    }
    #endregion
}
