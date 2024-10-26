﻿using Fvent.BO.Entities;
using Fvent.Repository.Common;
using System.Drawing.Printing;

namespace Fvent.Service.Specifications;

public static class EventSpec
{
    public class GetEventSpec : Specification<Event>
    {
        public GetEventSpec(string? searchKeyword, int? inMonth, int? inYear, string? eventType, string? eventTag,
                            string orderBy, bool isDescending, int pageNumber, int pageSize)
        {
            // Filter by search keyword (for event name or description)
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                Filter(e => e.EventName.Contains(searchKeyword) || e.Description.Contains(searchKeyword) || e.Organizer!.Username.Contains(searchKeyword));
            }

            // Filter by month for StartTime and EndTime
            if (inMonth.HasValue)
            {
                var month = inMonth.Value;
                var year = DateTime.UtcNow.Year;
                if (inYear.HasValue)
                {
                    year = inYear.Value;
                }
                
                Filter(e => (e.StartTime.Month == month && e.StartTime.Year == year || e.EndTime.Month == month && e.EndTime.Year == year));
            }

            // Filter by event type
            if (!string.IsNullOrEmpty(eventType))
            {
                Filter(e => e.EventType!.EventTypeName == eventType);
            }

            // Filter by event tag (assuming each event has an IList<EventTag> called Tags)
            if (!string.IsNullOrEmpty(eventTag))
            {
                Filter(e => e.Tags!.Any(tag => tag.Tag == eventTag));
            }

            if (orderBy is not null)
            {
                switch (orderBy)
                {
                    case "StartTime":
                        OrderBy(u => u.StartTime, isDescending);
                        break;
                    case "EndTime":
                        OrderBy(u => u.EndTime, isDescending);
                        break;
                    case "Name":
                        OrderBy(u => u.EventName, isDescending);
                        break;
                }
            }

            // Filter by event type
            if (!string.IsNullOrEmpty(eventType))
            {
                Filter(e => e.EventType!.EventTypeName == eventType);
            }

            AddPagination(pageNumber, pageSize);

            // Include related entities
            Include(e => e.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.EventMedia!);
            Include(e => e.Tags!);
        }

        public GetEventSpec(Guid id)
        {
            Filter(u => u.EventId == id);

            Include(u => u.Organizer!);
            Include(u => u.EventType!);
            Include(u => u.EventMedia!);
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
            Include(e => e.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.Tags!);
            Include(e => e.EventMedia!);
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
            Include(e => e.EventMedia!);
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
            Include(e => e.EventMedia!);
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
