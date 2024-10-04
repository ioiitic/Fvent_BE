using Fvent.Service.Request;
using Fvent.Service.Services;
using Fvent.Service.Services.Imp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;
    
[Route("api/users")]
[ApiController]
public class UsersController(IUserService userService,
                             INotificationService notificationService,
                             IEventFollowerService eventFollowerService) : ControllerBase
{
    #region User
    /// <summary>
    /// Controller for User Register
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserReq req)
    {
        var res = await userService.RegisterUser(req);

        return Ok(res);
    }
    #endregion

    #region Admin
    /// <summary>
    /// Controller for Admin Get list users info
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetList([FromQuery] GetListUsersReq req)
    {
        var res = await userService.GetListUsers(req);

        return Ok(res);
    }
    #endregion

    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetUser([FromQuery] Guid id)
    //{
    //    var res = await userService.GetListUsers();

    //    return Ok(res);
    //}

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserReq req)
    {
        var res = await userService.UpdateUser(id, req);

        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        await userService.DeleteUser(id);

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
