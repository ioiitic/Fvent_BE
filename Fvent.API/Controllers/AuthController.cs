using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IUserService userService) : ControllerBase
{
    #region Auth
    /// <summary>
    /// POST api/auth/login
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Authen([FromBody] AuthReq req)
    {
        var res = await userService.Authen(req);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(60)
        };
        Response.Cookies.Append("authToken", res.Token, cookieOptions);

        return Ok(res);
    }
    #endregion
}
