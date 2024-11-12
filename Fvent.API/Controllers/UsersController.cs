using Fvent.BO.Exceptions;
using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fvent.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserService userService, IEventService eventService,
                             INotificationService notificationService, 
                             IFollowerService eventFollowerService) : ControllerBase
{

    #region User
    /// <summary>
    /// Get list users info
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetList([FromQuery] GetListUsersReq req)
    {
        var res = await userService.GetList(req);

        return Ok(res);
    }

    /// <summary>
    /// Get own info
    /// </summary>
    /// <returns></returns>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetUserInfo()
    {
        var userId = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value!;
        var res = await userService.Get(Guid.Parse(userId));

        return Ok(res);
    }

    /// <summary>
    /// Get user info
    /// </summary>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
    {
        var res = await userService.Get(userId);

        return Ok(res);
    }

    /// <summary>
    /// Update info
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserReq req)
    {
        var userId = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value!;
        var res = await userService.Update(Guid.Parse(userId), req);

        return Ok(res);
    }

    /// <summary>
    /// Soft delete user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{userId}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        await userService.Delete(userId);

        return Ok();
    }
    #endregion

    #region User Account
    /// <summary>
    /// Register an account for user
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
    /// Add FPT CardId to User profile
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("addCard")]
    public async Task<IActionResult> AddCard([FromBody]AddCardIdRequest req)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }

        var res = await userService.AddCardId(userId, req.CardUrl);

        return Ok(res);
    }

    /// <summary>
    /// For Moderator approve User
    /// </summary>
    /// <param name="isApproved"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{userId}/approve")]
    public async Task<IActionResult> ApproveUser([FromRoute] Guid userId, [FromQuery] bool isApproved, [FromBody] ApproveUserRequest req)
    {
        var res = await userService.ApproveUser(userId, isApproved, req.ProcessNote);

        return Ok(res);
    }
    #endregion

    #region User Registration
    // TODO: Them field get theo thang, nam
    /// <summary>
    /// Get all events that user registered
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("participant")]
    public async Task<IActionResult> GetEventRegisters([FromQuery] bool isCompleted)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }
        var res = await eventService.GetRegisteredEvents(userId, isCompleted);

        return Ok(res);
    }
    #endregion

    #region Email
    /// <summary>
    /// Verify user via email
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] Guid userId, [FromQuery] string token)
    {
        await userService.VerifyEmailAsync(userId, token);
        return Ok("Email verified successfully!");
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

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid or missing user ID.");
        }

        try
        {
            await userService.ChangePasswordAsync(userId, request.OldPassword, request.NewPassword);
            return Ok("Password changed successfully.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (NotFoundException)
        {
            return NotFound("User not found.");
        }
    }

    #endregion

    #region User Notification
    [HttpGet("notifications")]
    public async Task<IActionResult> GetNotifications()
    {
        var userId = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value!;
        var res = await notificationService.GetListNotifications(Guid.Parse(userId));

        return Ok(res);
    }

    [HttpDelete("clear-notifications")]
    public async Task<IActionResult> ClearNoti()
    {
        var userId = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value!;
        await notificationService.ClearNotification(Guid.Parse(userId));

        return Ok();
    }

    [HttpGet("followed-events")]
    public async Task<IActionResult> GetFollowedEvents(Guid userId)
    {
        var res = await eventFollowerService.GetFollowedEvents(userId);

        return Ok(res);
    }
    #endregion
}
