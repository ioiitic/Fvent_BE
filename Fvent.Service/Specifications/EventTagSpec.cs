using Fvent.BO.Entities;
using Fvent.Repository.Common;

namespace Fvent.Service.Specifications;

public static class EventTagSpec
{
    public class GetEventTagSpec : Specification<EventTag>
    {
        public GetEventTagSpec(Guid eventId)
        {
            Filter(u => u.EventId == eventId);
        }
    }
}
