using Fvent.BO.Common;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IEventService
{
    #region Event
    /// <summary>
    /// Get list events
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<PageResult<EventRes>> GetListEvents(GetEventsRequest req);
    #endregion



    #region Student
    Task<PageResult<EventRes>> GetListRecommend(IdReq req);
    #endregion

    #region CRUD Event
    Task<EventRes> GetEvent(Guid id);
    Task<IList<EventRes>> GetListEventsByOrganizer(Guid organizerId);
    Task<IdRes> CreateEvent(CreateEventReq req);
    Task<IdRes> UpdateEvent(Guid id, UpdateEventReq req);
    Task DeleteEvent(Guid id);
    #endregion

    #region Event-User
    /// <summary>
    /// Test commmand
    /// </summary>
    /// <param name="req"></param>
    /// <returns>Tra ve <see cref="IList{UserRes}"/></returns>
    Task<IList<UserRes>> GetEventRegisters(Guid req);
    #endregion
}
