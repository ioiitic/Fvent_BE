using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers
{
    [Route("api/event-types")]
    [ApiController]
    public class EventTypesController(IEventTypeService eventTypeService) : ControllerBase
    {
        #region EventType
        /// <summary>
        /// Get list event types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var res = await eventTypeService.GetListEventTypes();

            return Ok(res);
        }

        /// <summary>
        /// Get event type
        /// </summary>
        /// <param name="eventTypeId"></param>
        /// <returns></returns>
        [HttpGet("{eventTypeId}")]
        public async Task<IActionResult> GetEventType([FromRoute] Guid eventTypeId)
        {
            var res = await eventTypeService.GetEventType(eventTypeId);

            return Ok(res);
        }

        /// <summary>
        /// Create an event type
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateEventType([FromBody] CreateEventTypeReq req)
        {
            var res = await eventTypeService.CreateEventType(req.eventTypeName);

            return Ok(res);
        }

        /// <summary>
        /// Update an event type
        /// </summary>
        /// <param name="eventTypeId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("{eventTypeId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateEventType([FromRoute] Guid eventTypeId, [FromBody] UpdateEventTypeReq req)
        {
            var res = await eventTypeService.UpdateEventType(eventTypeId, req.eventTypeName);

            return Ok(res);
        }

        /// <summary>
        /// Delete an event type
        /// </summary>
        /// <param name="eventTypeId"></param>
        /// <returns></returns>
        [HttpDelete("{eventTypeId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteEventType([FromRoute] Guid eventTypeId)
        {
            await eventTypeService.DeleteEventType(eventTypeId);

            return Ok();
        }
        #endregion
    }
}
