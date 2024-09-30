using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(IUserService userService,
                             INotificationService notificationService) : ControllerBase
{
    #region User
    /// <summary>
    /// Register user controller
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserReq req)
    {
        var res = await userService.RegisterUser(req);

        return Ok(res);
    }

    /// <summary>
    /// Get list Users by Admin
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetList()
    {
        var res = await userService.GetListUsers();

        return Ok(res);
    }
    #endregion

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser([FromQuery] Guid id)
    {
        var res = await userService.GetListUsers();

        return Ok(res);
    }

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
}
