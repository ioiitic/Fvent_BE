using Fvent.BO.Entities;
using Fvent.Repository.Common;

namespace Fvent.Service.Specifications;

public static class EventFollowerSpec
{
    public class GetEventFollowerSpec : Specification<EventFollower>
    {
        public GetEventFollowerSpec(Guid eventId, Guid userId)
        {
            Filter(u => u.EventId == eventId && u.UserId == userId);
        }
    }
}
