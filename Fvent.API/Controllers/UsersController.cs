﻿using Fvent.Service.Request;
using Fvent.Service.Services;
using Fvent.Service.Services.Imp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fvent.API.Controllers;

[ApiController]
public class UsersController(IUserService userService, IEventService eventService,
                             INotificationService notificationService, 
                             IEventFollowerService eventFollowerService) : ControllerBase
{
    [HttpGet("api/users/verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] Guid userId, [FromQuery] string token)
    {
        var result = await userService.VerifyEmailAsync(userId, token);
        if (result)
        {
            return Ok("Email verified successfully!");
        }
        else
        {
            return BadRequest("Email verification failed.");
        }
    }
    #region User
    /// <summary>
    /// Controller for User Register
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("api/users/register")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserReq req)
    {
        var res = await userService.Register(req);

        return Ok(res);
    }

    /// <summary>
    /// Controller for User Get own info
    /// </summary>
    /// <returns></returns>
    [HttpGet("api/user/me")]
    [Authorize]
    public async Task<IActionResult> GetUserInfo()
    {
        var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var res = await userService.GetByEmail(email!);

        return Ok(res);
    }

   /// <summary>
   /// Controller for User Update info
   /// </summary>
   /// <param name="id"></param>
   /// <param name="req"></param>
   /// <returns></returns>
    [HttpPut("api/users/{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserReq req)
    {
        var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var user = await userService.GetByEmail(email!);

        var res = await userService.Update(user.UserId, req);

        return Ok(res);
    }
    #endregion

    #region Student
    [HttpGet("api/users/recommendation")]
    public async Task<IActionResult> GetListRecommend()
    {
        var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var user = await userService.GetByEmail(email!);

        var res = await eventService.GetListRecommend(new IdReq(user.UserId));

        return Ok(res);
    }
    #endregion

    #region Admin
    /// <summary>
    /// Controller for Admin Get list users info
    /// </summary>
    /// <returns></returns>
    [HttpGet("api/users")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetList([FromQuery] GetListUsersReq req)
    {
        var res = await userService.GetList(req);

        return Ok(res);
    }
    #endregion

    [HttpDelete("api/users/{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        await userService.Delete(id);

        return Ok();
    }

    [HttpGet("api/users/{id}/notifications")]
    public async Task<IActionResult> GetList(Guid id)
    {
        var res = await notificationService.GetListNotifications(id);

        return Ok(res);
    }

    [HttpDelete("api/users/{id}/clear-notification")]
    public async Task<IActionResult> ClearNoti(Guid id)
    {
        await notificationService.ClearNotification(id);

        return Ok();
    }

    [HttpGet("api/users/{userId}/followed-events")]
    public async Task<IActionResult> GetFollowedEvents(Guid userId)
    {
        var res = await eventFollowerService.GetFollowedEvents(userId);
        return Ok(res);
    }

}
