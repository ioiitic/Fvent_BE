using Fvent.Service.Request;
using Fvent.Service.Services;
using Fvent.Service.Services.Imp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fvent.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserService userService, IEventService eventService,
                             INotificationService notificationService, 
                             IFollowerService eventFollowerService,
                             IRegistationService registationService) : ControllerBase
{
    #region Email
    /// <summary>
    /// GET api/users/verify-email
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("verify-email")]
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

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordReq request)
    {
        await userService.RequestPasswordResetAsync(request.email);
        return Ok("Password reset link has been sent.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromQuery] Guid userId, [FromQuery] string token, [FromBody] string newPassword)
    {
        await userService.ResetPasswordAsync(userId, token, newPassword);
        return Ok("Password has been reset successfully.");
    }
    #endregion


    #region User
    /// <summary>
    /// Controller for User Register
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserReq req)
    {
        var res = await userService.Register(req);

        return Ok(res);
    }

    /// <summary>
    /// Controller for User Get own info
    /// </summary>
    /// <returns></returns>
    [HttpGet("me")]
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
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserReq req)
    {
        var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var user = await userService.GetByEmail(email!);

        var res = await userService.Update(user.UserId, req);

        return Ok(res);
    }
    #endregion

    #region Student
    #endregion

    #region Admin
    /// <summary>
    /// Controller for Admin Get list users info
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetList([FromQuery] GetListUsersReq req)
    {
        var res = await userService.GetList(req);

        return Ok(res);
    }
    #endregion

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        await userService.Delete(id);

        return Ok();
    }

    [HttpGet("{id}/notifications")]
    public async Task<IActionResult> GetList(Guid id)
    {
        var res = await notificationService.GetListNotifications(id);

        return Ok(res);
    }

    [HttpDelete("{id}/clear-notification")]
    public async Task<IActionResult> ClearNoti(Guid id)
    {
        await notificationService.ClearNotification(id);

        return Ok();
    }

    [HttpGet("{userId}/followed-events")]
    public async Task<IActionResult> GetFollowedEvents(Guid userId)
    {
        var res = await eventFollowerService.GetFollowedEvents(userId);
        return Ok(res);
    }
}
