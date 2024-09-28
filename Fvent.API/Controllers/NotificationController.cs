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
    public async Task<IActionResult> CreateReview([FromBody] CreateNotificationReq req)
    {
        var res = await notificationService.CreateNotification(req);

        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
    {
        await notificationService.DeleteNotification(id);

        return Ok();
    }
}
