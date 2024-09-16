using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
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

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserReq req)
    {
        var res = await userService.CreateUser(req);

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
}
