using Fvent.BO.Common;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IEventService
{
    Task<PageResult<EventRes>> GetListEvents(GetEventsRequest req);
    Task<EventRes> GetEvent(Guid id);
    Task<IdRes> CreateEvent(CreateEventReq req);
    Task<IdRes> UpdateEvent(Guid id, UpdateEventReq req);
    Task<IdRes> FollowEvent(Guid id, Guid userId);
    Task UnfollowEvent(Guid eventId, Guid userId);
    Task<IdRes> RegisterFreeEvent(Guid id, Guid userId);
    Task DeleteEvent(Guid id);
}
