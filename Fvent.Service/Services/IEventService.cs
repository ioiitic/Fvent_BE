using Fvent.BO.Common;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IEventService
{
    Task<PageResult<EventRes>> GetListEvents(GetEventsRequest req);
    Task<EventRes> GetEvent(Guid id);
    Task<IList<EventRes>> GetListEventsByOrganizer(Guid organizerId);
    Task<PageResult<EventRes>> GetListRecommend(IdReq req);
    Task<IdRes> CreateEvent(CreateEventReq req);
    Task<IdRes> UpdateEvent(Guid id, UpdateEventReq req);
    Task DeleteEvent(Guid id);
    Task<IList<UserRes>> GetEventRegisters(Guid req);
}
