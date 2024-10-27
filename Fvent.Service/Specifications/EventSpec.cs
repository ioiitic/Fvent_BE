using Fvent.BO.Entities;
using Fvent.Repository.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Fvent.Service.Specifications;

public static class EventSpec
{
    public class GetEventSpec : Specification<Event>
    {
        public GetEventSpec(string? searchKeyword, DateTime? fromDate, DateTime? toDate, string? eventType)
        {
            // Filter by search keyword (for event name or description)
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                Filter(e => e.EventName.Contains(searchKeyword) || e.Description.Contains(searchKeyword));
            }

            // Filter by date range
            if (fromDate.HasValue)
            {
                Filter(e => e.StartTime >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                Filter(e => e.EndTime <= toDate.Value);
            }

            // Filter by event type
            if (!string.IsNullOrEmpty(eventType))
            {
                Filter(e => e.EventType!.EventTypeName == eventType);
            }

            // Include related entities
            Include(e => e.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.EventMedias!);
        }

        public GetEventSpec(Guid id)
        {
            Filter(u => u.EventId == id);

            Include(u => u.Organizer!);
            Include(u => u.EventType!);
            Include(u => u.EventMedias!);
        }
    }

    public class GetEventRateSpec : Specification<EventReview>
    {
        public GetEventRateSpec(Guid id)
        {
            Filter(er => er.EventId == id);
        }
    }

    public class GetRegisteredEventsSpec : Specification<Event>
    {
        public GetRegisteredEventsSpec(Guid userId)
        {
            Filter(e => e.Registrations!.Any(r => r.UserId == userId));

            Include("Registrations.User.Role");
            Include(e => e.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.Tags!);
            Include(e => e.EventMedias!);
        }

    }

    public class GetRegisteredUsersSpec : Specification<Event>
    {
        public GetRegisteredUsersSpec(Guid eventId)
        {
            Filter(e => e.EventId == eventId);

            Include("Registrations.User.Role");
            Include(e => e.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.Tags!);
            Include(e => e.EventMedias!);
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

    public class GetListRecommend : Specification<Event>
    {
        public GetListRecommend(IEnumerable<Guid> eventTypes, IEnumerable<string> eventTags)
        {
            Filter(e => eventTypes.Any(t => e.EventTypeId == t) || eventTags.Any(type => e.Tags!.Any(tag => tag.Tag.Equals(type))));

            Include(e => e.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.Tags!);
            Include(e => e.EventMedias!);
        }
    }

    public class GetEventByOrganizerSpec : Specification<Event>
    {
        public GetEventByOrganizerSpec(Guid Id)
        {
            Filter(e => e.OrganizerId == Id);

            Include(e => e.Tags!);
            Include(u => u.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.EventMedias!);
        }
    }
    public class GetUserFollowsEventSpec : Specification<EventFollower>
    {
        public GetUserFollowsEventSpec(Guid eventId)
        {
            Filter(f => f.EventId == eventId);

            // Include the related Event entity
            Include(f => f.Event!);
            Include(f => f.Event!.Organizer!);
            Include(f => f.Event!.EventType!);
        }
    }
}
