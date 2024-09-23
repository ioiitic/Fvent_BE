using Fvent.BO.Entities;
using Fvent.Repository.Common;

namespace Fvent.Service.Specifications;

public static class EventSpec
{
    public class GetEventSpec : Specification<Event>
    {
        public GetEventSpec(string? searchKeyword, string campus, DateTime? fromDate, DateTime? toDate, string? eventType)
        {
            // Filter by search keyword (for event name or description)
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                Filter(e => e.EventName.Contains(searchKeyword) || e.Description.Contains(searchKeyword));
            }

            // Filter by campus
            if (!string.IsNullOrEmpty(campus))
            {
                Filter(e => e.Campus == campus);
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
        }

        public GetEventSpec(Guid id)
        {
            Filter(u => u.EventId == id);

            Include(u => u.Organizer!);
            Include(u => u.EventType!);
        }
    }
}
