using Fvent.BO.Entities;
using Fvent.Repository.Common;

namespace Fvent.Service.Specifications;

public static class EventRegistationSpec
{
    public class GetEventParticipantsSpec : Specification<EventRegistration>
    {
        public GetEventParticipantsSpec(Guid eventId)
        {
            // Filter to get participants for the given eventId
            Filter(p => p.EventId == eventId);

            // Include the User and User Role entities
            Include(p => p.User!);
            Include(p => p.User!.Role!);
        }
    }
}
