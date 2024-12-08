using Fvent.BO.Entities;
using Fvent.Repository.Common;

namespace Fvent.Service.Specifications;

public static class EventRegistationSpec
{
    public class GetEventParticipantsSpec : Specification<EventRegistration>
    {
        public GetEventParticipantsSpec(Guid eventId)
        {
            Filter(p => p.EventId == eventId);

            Include(p => p.User!);
            Include(p => p.User!.Role!);
        }
    }

    public class GetEventRegistrationSpec : Specification<EventRegistration>
    {
        public GetEventRegistrationSpec(Guid eventId, Guid userId)
        {
            Filter(p => p.EventId == eventId && p.UserId == userId);

            Include(p => p.Event!);
        }
        public GetEventRegistrationSpec(Guid? userId,Guid eventId, DateTime startTime, DateTime endTime)
        {
            Filter(p => p.UserId == userId && p.EventId != eventId
                    && p.Event.StartTime < endTime
                    && p.Event.EndTime > startTime);
            Include(p => p.Event!);
        }

    }

    public class GetListUserEventsSpec : Specification<EventRegistration>
    {
        public GetListUserEventsSpec(Guid userId)
        {
            Filter(r => r.UserId == userId);

            Include("Event.Tags");
            Include("Event.Organizer");
            Include("Event.EventType");
        }
    }
}
