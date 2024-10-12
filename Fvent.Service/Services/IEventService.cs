﻿using Fvent.BO.Common;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IEventService
{
    #region Student
    Task<PageResult<EventRes>> GetListRecommend(IdReq req);
    #endregion

    #region CRUD Event
    Task<PageResult<EventRes>> GetListEvents(GetEventsRequest req);
    Task<EventRes> GetEvent(Guid id);
    Task<IList<EventRes>> GetListEventsByOrganizer(Guid organizerId);
    Task<IdRes> CreateEvent(CreateEventReq req);
    Task<IdRes> UpdateEvent(Guid id, UpdateEventReq req);
    Task DeleteEvent(Guid id);
    #endregion

    #region Event
    Task<EventRateRes> GetEventRate(IdReq req);
    #endregion

    #region Event-Review
    Task<IdRes> CreateReview(CreateReviewReq req);
    Task<IList<ReviewRes>> GetEventReviews(IdReq req);
    #endregion

    #region Event-User
    Task<IList<UserRes>> GetEventRegisters(IdReq req);
    #endregion
}
