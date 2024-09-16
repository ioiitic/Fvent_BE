using Fvent.BO.Entities;
using Fvent.Repository.Common;

namespace Fvent.Service.Specifications;

public static class EventSpec
{
    public class GetEventSpec : Specification<Event>
    {
        public GetEventSpec()
        {
            Include(u => u.Organizer!);
            Include(u => u.EventType!);
        }
        public GetEventSpec(Guid id)
        {
            Filter(u => u.EventId == id);

            Include(u => u.Organizer!);
            Include(u => u.EventType!);
        }
    }
}
