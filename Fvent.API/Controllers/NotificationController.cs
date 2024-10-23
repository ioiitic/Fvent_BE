﻿using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/notifications")]
[ApiController]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    #region Notification
    /// <summary>
    /// GET api/notifications/{userId}
    /// Get all notifications of user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetNotifications(Guid userId)
    {
        var res = await notificationService.GetNotification(userId);

        return Ok(res);
    }

    /// <summary>
    /// POST api/notifications
    /// Post a notification
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateNoti([FromBody] CreateNotificationReq req)
    {
        var res = await notificationService.CreateNotification(req);

        return Ok(res);
    }

    /// <summary>
    /// PUT api/notifications/{notiId}/read
    /// Update a notification status when user read it
    /// </summary>
    /// <param name="notiId"></param>
    /// <returns></returns>
    [HttpPut("{notiId}/read")]
    public async Task<IActionResult> ReadNoti(Guid notiId)
    {
        var res = await notificationService.ReadNotification(notiId);

        return Ok(res);
    }

    /// <summary>
    /// DELETE api/notifications/{notiId}
    /// Delete a notification
    /// </summary>
    /// <param name="notiId"></param>
    /// <returns></returns>
    [HttpDelete("{notiId}")]
    public async Task<IActionResult> DeleteNoti([FromRoute] Guid notiId)
    {
        await notificationService.DeleteNotification(id);

        return Ok();
    }
    #endregion
}
