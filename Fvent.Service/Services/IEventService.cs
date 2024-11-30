using Fvent.BO.Common;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IEventService
{
    Task<PageResult<EventRes>> GetListEvents(GetEventsRequest req);
    Task<PageResult<EventRes>> GetListEventsForAdmin(GetEventsRequest req);
    Task<List<EventBannerRes>> GetEventBanners();
    Task<EventRes> GetEvent(Guid eventId, Guid? userId);
    Task<IList<EventRes>> GetListEventsByOrganizer(GetEventByOrganizerReq req);
    Task<PageResult<EventRes>> GetListEventsOfOrganizer(GetEventOfOrganizerReq req);
    Task<PageResult<EventRes>> GetListRecommend(Guid userId);
    Task<IdRes> CreateEvent(CreateEventReq req, Guid organizerId);
    Task<IdRes> UpdateEvent(Guid id, Guid organizerId, UpdateEventReq req);
    Task<IdRes> SubmitEvent(Guid id, Guid organizerId);
    Task<IdRes> ApproveEvent(Guid id, bool isApproved,Guid userId, string processNote);
    Task CheckinEvent(Guid eventId, Guid userId, bool isOrganizer);
    Task DeleteEvent(Guid id);
    Task<PageResult<UserRes>> GetRegisteredUsers(Guid eventId, GetRegisteredUsersReq req, Guid userId);
    Task<IList<EventRes>> GetRegisteredEvents(Guid userId,int? inMonth, int? inYear, bool isCompleted);
}
