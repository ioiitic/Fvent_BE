using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers
{
    [Route("api/event-types")]
    [ApiController]
    public class EventTypesController(IEventTypeService eventTypeService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var res = await eventTypeService.GetListEventTypes();

            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventType(Guid id)
        {
            var res = await eventTypeService.GetEventType(id);

            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventType([FromBody] CreateEventTypeReq req)
        {
            var res = await eventTypeService.CreateEventType(req.eventTypeName);

            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventType([FromRoute] Guid id, [FromBody] UpdateEventTypeReq req)
        {
            var res = await eventTypeService.UpdateEventType(id, req.eventTypeName);

            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventType([FromRoute] Guid id)
        {
            await eventTypeService.DeleteEventType(id);

            return Ok();
        }
    }
}
