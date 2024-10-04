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

        public GetEventFollowerSpec(Guid userId)
        {
            // Filter by userId to get the events followed by this user
            Filter(f => f.UserId == userId);

            // Include the related Event entity
            Include(f => f.Event!);
            Include(f => f.Event!.Organizer!);
            Include(f => f.Event!.EventType!);
        }
    }
}
