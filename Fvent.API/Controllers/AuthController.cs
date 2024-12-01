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
    /// Login
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Authen([FromBody] AuthReq req)
    {
        // get source ip address for the current request
        string ipAddress;
        ipAddress = HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();

        var res = await userService.Authen(req, ipAddress);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(60)
        };
        Response.Cookies.Append("authToken", res.Token, cookieOptions);
        Response.Cookies.Append("refreshToken", res.RefreshToken, cookieOptions);

        return Ok(res);
    }

    /// <summary>
    /// Refresh token
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenReq req)
    {
        // get source ip address for the current request
        string ipAddress;
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            ipAddress = Request.Headers["X-Forwarded-For"]!;
        else
            ipAddress = HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();

        var res = await userService.Refresh(req, ipAddress);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(60)
        };
        Response.Cookies.Append("authToken", res.Token, cookieOptions);
        Response.Cookies.Append("refreshToken", res.RefreshToken, cookieOptions);

        return Ok(res);

    }
    #endregion
}
