using Fvent.BO.Entities;
using Fvent.BO.Enums;
using Fvent.Repository.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing.Printing;
using System.Text;

namespace Fvent.Service.Specifications;

public static class EventSpec
{
    public class GetEventSpec : Specification<Event>
    {
        public GetEventSpec(string? searchKeyword, int? inMonth, int? inYear, List<string>? eventTypes, string? eventTag,
                            string? status, string orderBy, bool isDescending, int pageNumber, int pageSize)
        {
            Filter(e => e.Status == EventStatus.Upcoming || e.Status == EventStatus.InProgress || e.Status == EventStatus.Completed);
            // Filter by search keyword (for event name or description)
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    Filter(e => EF.Functions.Collate(e.EventName, "SQL_Latin1_General_CP1_CI_AI").Contains(searchKeyword) ||
                                EF.Functions.Collate(e.Organizer!.Username, "SQL_Latin1_General_CP1_CI_AI").Contains(searchKeyword) ||
                                e.Tags!.Any(tag => EF.Functions.Collate(tag.Tag, "SQL_Latin1_General_CP1_CI_AI").Contains(searchKeyword)));
                }

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

                if(eventStatus == EventStatus.Completed)
                {
                    OrderBy(u => u.EndTime, true);
                }
                else
                {
                    OrderBy(u => u.StartTime, false);
                }
            }
            else
            {
                OrderBy(u => u.StartTime, true);
            }

            AddPagination(pageNumber, pageSize);

            // Include related entities
            Include(e => e.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.EventMedias!);
            Include(e => e.Tags!);
            Include(e => e.EventFile!);
        }

        public GetEventSpec(Guid id)
        {
            Filter(u => u.EventId == id);

            Include(u => u.Organizer!);
            Include(u => u.EventType!);
            Include(u => u.EventMedias!);
            Include(e => e.Tags!);
            Include(e => e.EventFile!);
            Include(e => e.Form!.FormDetails!);
        }

        public GetEventSpec()
        {
            Filter(e => e.Status == EventStatus.Upcoming || e.Status == EventStatus.InProgress || e.Status == EventStatus.Completed);
            OrderBy(u => u.StartTime, true);

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

    public class GetEventAdminSpec : Specification<Event>
    {
        public GetEventAdminSpec(string? searchKeyword, int? inMonth, int? inYear, List<string>? eventTypes, string? eventTag,
                            string? status, string orderBy, bool isDescending, int pageNumber, int pageSize)
        {
            Filter(e => e.Status != EventStatus.Draft);
            // Filter by search keyword (for event name or description)
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                Filter(e => e.EventName.Contains(searchKeyword) || e.Organizer!.Username.Contains(searchKeyword)
                                                                || e.Tags!.Any(tag => tag.Tag.Contains(searchKeyword)));
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

                if (eventStatus == EventStatus.Completed)
                {
                    OrderBy(u => u.EndTime, true);
                }
                else
                {
                    OrderBy(u => u.StartTime, false);
                }
            }
            else
            {
                OrderBy(u => u.StartTime, true);
            }

            AddPagination(pageNumber, pageSize);

            // Include related entities
            Include(e => e.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.EventMedias!);
            Include(e => e.Tags!);
            Include(e => e.EventFile!);
        }
    }

    public class GetRegisteredEventsSpec : Specification<Event>
    {
        public GetRegisteredEventsSpec(Guid userId, int? inMonth, int? inYear, bool isCompleted)
        {
            // Filter by month for StartTime and EndTime
            if (inMonth.HasValue)
            {
                var month = inMonth.Value;
                var year = inYear ?? DateTime.UtcNow.Year;

                Filter(e => (e.StartTime.Month == month && e.StartTime.Year == year || e.EndTime.Month == month && e.EndTime.Year == year));
            }

            Filter(e => e.Registrations!.Any(r => r.UserId == userId));

            if (isCompleted)
            {
                Filter(e => e.Status == EventStatus.Completed);
            }
            else
            {
                Filter(e => e.Status == EventStatus.Upcoming || e.Status == EventStatus.InProgress);
            }
            OrderBy(u => u.StartTime, true);
            Include("Registrations.User.Role");
            Include(e => e.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.Tags!);
            Include(e => e.EventMedias!);
            Include(e => e.EventFile!);
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
            Include(e => e.Registrations!);
        }
    }

    public class GetEventReminderSpec : Specification<Event>
    {
        public GetEventReminderSpec(int ReminderThresholdMinutes)
        {
            Filter(e => e.Status == EventStatus.Upcoming && e.StartTime > DateTime.Now &&
                                                          e.StartTime <= DateTime.Now.AddMinutes(ReminderThresholdMinutes));
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
            Filter(e => e.Status == EventStatus.Upcoming || e.Status == EventStatus.InProgress);
            Filter(e => eventTypes.Any(t => e.EventTypeId == t) || eventTags.Any(type => e.Tags!.Any(tag => tag.Tag.Equals(type))));

            Include(e => e.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.Tags!);
            Include(e => e.EventMedias!);
            Include(e => e.EventFile!);
        }
    }

    public class GetEventByOrganizerSpec : Specification<Event>
    {
        public GetEventByOrganizerSpec(Guid userId, string? status)
        {
            Filter(e => e.Status == EventStatus.Upcoming || e.Status == EventStatus.InProgress || e.Status == EventStatus.Completed);

            Filter(e => e.OrganizerId == userId);

            // Filter by event status if provided
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<EventStatus>(status, true, out var eventStatus))
            {
                Filter(e => e.Status == eventStatus);
            }
            OrderBy(u => u.StartTime, true);

            Include(e => e.Tags!);
            Include(u => u.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.EventMedias!);
            Include(e => e.EventFile!);
        }

        public GetEventByOrganizerSpec(Guid userId, string? searchKeyword, int? inMonth, int? inYear,
                                       List<string>? eventTypes, string? eventTag, string? status, int pageNumber,
                                       int pageSize)
        {
            Filter(e => e.OrganizerId == userId);
            // Filter by search keyword (for event name or description)
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    Filter(e => EF.Functions.Collate(e.EventName, "SQL_Latin1_General_CP1_CI_AI").Contains(searchKeyword) ||
                                EF.Functions.Collate(e.Organizer!.Username, "SQL_Latin1_General_CP1_CI_AI").Contains(searchKeyword) ||
                                e.Tags!.Any(tag => EF.Functions.Collate(tag.Tag, "SQL_Latin1_General_CP1_CI_AI").Contains(searchKeyword)));
                }

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

                if (eventStatus == EventStatus.Completed)
                {
                    OrderBy(u => u.EndTime, true);
                }
                else
                {
                    OrderBy(u => u.StartTime, false);
                }
            }
            else
            {
                OrderBy(u => u.StartTime, true);
            }

            AddPagination(pageNumber, pageSize);

            // Include related entities
            Include(e => e.Organizer!);
            Include(e => e.EventType!);
            Include(e => e.EventMedias!);
            Include(e => e.Tags!);
            Include(e => e.EventFile!);
        }
    }
    public class GetEventForReportSpec : Specification<Event>
    {
        public GetEventForReportSpec(DateTime? startDate = null, DateTime? endDate = null)
        {
            Filter(e => e.Status == EventStatus.Upcoming || e.Status == EventStatus.InProgress || e.Status == EventStatus.Completed);

            if (startDate is not null && endDate is not null)
            {
                Filter(e => e.EndTime <= endDate && e.EndTime >= startDate);

                Include("Registrations.User");
            }

            OrderBy(u => u.StartTime, true);
        }

        public GetEventForReportSpec(Guid userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            Filter(e => e.Status == EventStatus.Upcoming || e.Status == EventStatus.InProgress || e.Status == EventStatus.Completed);

            Filter(e => e.OrganizerId == userId);

            if (startDate is not null && endDate is not null)
            {
                Filter(e => e.EndTime <= endDate && e.EndTime >= startDate);

                Include("Registrations.User");
            }

            OrderBy(u => u.StartTime, true);
        }
    }
}
