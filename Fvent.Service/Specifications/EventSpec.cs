using Fvent.BO.Entities;
using Fvent.BO.Enums;
using Fvent.Repository.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing.Printing;

namespace Fvent.Service.Specifications;

public static class EventSpec
{
    public class GetEventSpec : Specification<Event>
    {
        public GetEventSpec(string? searchKeyword, int? inMonth, int? inYear, List<string>? eventTypes, string? eventTag,
                            string? status, string orderBy, bool isDescending, int pageNumber, int pageSize)
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
                var year = inYear ?? DateTime.UtcNow.Year;

                Filter(e => (e.StartTime.Month == month && e.StartTime.Year == year || e.EndTime.Month == month && e.EndTime.Year == year));
            }
            else if (!inMonth.HasValue && inYear.HasValue)
            {
                var year = inYear.Value;
                Filter(e => (e.StartTime.Year == year ||  e.EndTime.Year == year));
            }

            // Filter by multiple event types if provided
            if (eventTypes != null && eventTypes.Any())
            {
                Filter(e => eventTypes.Contains(e.EventType!.EventTypeName));
            }

            // Filter by event tag (assuming each event has an IList<EventTag> called Tags)
            if (!string.IsNullOrEmpty(eventTag))
            {
                Filter(e => e.Tags!.Any(tag => tag.Tag == eventTag));
            }

            // Filter by event status if provided
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<EventStatus>(status, true, out var eventStatus))
            {
                Filter(e => e.Status == eventStatus);
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

            AddPagination(pageNumber, pageSize);

            // Include related entities
            Include(e => e.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.EventMedias!);
            Include(e => e.Tags!);
        }

        public GetEventSpec(Guid id)
        {
            Filter(u => u.EventId == id);

            Include(u => u.Organizer!);
            Include(u => u.EventType!);
            Include(u => u.EventMedias!);
            Include(e => e.Tags!);
            Include(e => e.Form!.FormDetails!);
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
        public GetRegisteredEventsSpec(Guid userId, bool isCompleted)
        {
            
            Filter(e => e.Registrations!.Any(r => r.UserId == userId));

            if (isCompleted)
            {
                Filter(e => e.Status == EventStatus.Completed);
            }
            else
            {
                Filter(e => e.Status == EventStatus.Upcoming || e.Status == EventStatus.InProgress);
            }

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
        public GetEventByOrganizerSpec(Guid Id, string? status)
        {
            Filter(e => e.OrganizerId == Id);

            // Filter by event status if provided
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<EventStatus>(status, true, out var eventStatus))
            {
                Filter(e => e.Status == eventStatus);
            }

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
