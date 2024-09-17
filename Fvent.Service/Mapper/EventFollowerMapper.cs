using Fvent.BO.Entities;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Mapper;


public static class EventFollowerMapper
{
    public static EventFollower ToEventFollower(this FollowEventReq src, Guid eventId)
        => new(
            eventId,
            src.userId
            );
}
