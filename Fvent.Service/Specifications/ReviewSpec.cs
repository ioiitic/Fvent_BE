using Fvent.BO.Entities;
using Fvent.Repository.Common;

namespace Fvent.Service.Specifications;

public static class ReviewSpec
{
    public class GetReviewSpec : Specification<EventReview>
    {
        public GetReviewSpec()
        {
            Include(u => u.User!);
            Include(u => u.Event!);
        }
        
        public GetReviewSpec(Guid eventId, Guid? userId)
        {
            Filter(u => u.EventId == eventId && u.UserId == userId);
        }
        public GetReviewSpec(Guid id)
        {
            Filter(u => u.EventReviewId == id);

            Include(u => u.User!);
            Include(u => u.Event!);
        }
    }

    public class GetReviewByEventSpec : Specification<EventReview>
    {
        public GetReviewByEventSpec(Guid id)
        {
            Filter(u => u.EventId == id);
            OrderBy(u => u.CreatedAt, true);

            Include(u => u.User!);
            Include(u => u.Event!);
        }
    }
}
