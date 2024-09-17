using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/_events")]
[ApiController]
public class EventsController(IEventService _eventService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var res = await _eventService.GetListEvents();

        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var res = await _eventService.GetListEvents();

        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventReq req)
    {
        var res = await _eventService.CreateEvent(req);

        return Ok(res);
    }

    [HttpPost("{id}/follow")]
    public async Task<IActionResult> FollowEvent(Guid id, [FromBody] FollowEventReq req)
    {
        var res = await _eventService.FollowEvent(id, req);

        return Ok(res);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent([FromRoute] Guid id, [FromBody] UpdateEventReq req)
    {
        var res = await _eventService.UpdateEvent(id, req);

        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent([FromRoute] Guid id)
    {
        await _eventService.DeleteEvent(id);

        return Ok();
    }
}
