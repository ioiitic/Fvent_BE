using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IUserService userService) : ControllerBase
{
    #region User
    /// <summary>
    /// Controller for User Login
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Authen([FromBody] AuthReq req)
    {
        // Call service
        var res = await userService.Authen(req);

        // Assign token to cookies response
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
