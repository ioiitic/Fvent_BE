using Fvent.Service.Request;
using Fvent.Service.Services;
using Fvent.Service.Services.Imp;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Authen([FromBody] AuthReq req)
    {
        var res = await userService.Authen(req);

        return Ok(res);
    }
}
