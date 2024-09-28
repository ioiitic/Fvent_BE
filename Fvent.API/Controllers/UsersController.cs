using Fvent.Service.Request;
using Fvent.Service.Services;
using Fvent.Service.Services.Imp;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(IUserService userService,
                             INotificationService notificationService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var res = await userService.GetListUsers();

        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser([FromQuery] Guid id)
    {
        var res = await userService.GetListUsers();

        return Ok(res);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserReq req)
    {
        var res = await userService.RegisterUser(req);

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
