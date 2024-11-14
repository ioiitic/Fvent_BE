﻿using Fvent.BO.Common;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IEventService
{
    Task<PageResult<EventRes>> GetListEvents(GetEventsRequest req);
    Task<List<EventBannerRes>> GetEventBanners();
    Task<EventRes> GetEvent(Guid eventId, Guid? userId);
    Task<IList<EventRes>> GetListEventsByOrganizer(GetEventByOrganizerReq req);
    Task<PageResult<EventRes>> GetListRecommend(Guid userId);
    Task<IdRes> CreateEvent(CreateEventReq req, Guid organizerId);
    Task<IdRes> UpdateEvent(Guid id, Guid organizerId, UpdateEventReq req);
    Task<IdRes> SubmitEvent(Guid id);
    Task<IdRes> ApproveEvent(Guid id, bool isApproved, string processNote);
    Task CheckinEvent(Guid eventId, Guid userId);
    Task DeleteEvent(Guid id);
    Task<IList<UserRes>> GetRegisteredUsers(Guid eventId);
    Task<IList<EventRes>> GetRegisteredEvents(Guid userId, bool isCompleted);
}
