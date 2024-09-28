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

    public class GetEventRateSpec : Specification<EventReview>
    {
        public GetEventRateSpec(Guid Id)
        {
            Filter(er => er.EventId == Id);
        }
    }

    public class GetEventRegistersSpec : Specification<Event>
    {
        public GetEventRegistersSpec(Guid Id)
        {
            Filter(e => e.EventId == Id);

            Include("Registration.User");
        }
    }

    public class GetEventReviewsSpec : Specification<EventReview>
    {
        public GetEventReviewsSpec(Guid Id)
        {
            Filter(e => e.EventId == Id);

            Include(e => e.User!);
        }
    }
}
