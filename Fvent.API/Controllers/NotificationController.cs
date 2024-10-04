using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/notifications")]
[ApiController]
public class NotificationController(INotificationService notificationService) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNotifications(Guid id)
    {
        var res = await notificationService.GetNotification(id);

        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNoti([FromBody] CreateNotificationReq req)
    {
        var res = await notificationService.CreateNotification(req);

        return Ok(res);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> ReadNoti(Guid id)
    {
        var res = await notificationService.ReadNotification(id);

        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNoti([FromRoute] Guid id)
    {
        await notificationService.DeleteNotification(id);

        return Ok();
    }
}
