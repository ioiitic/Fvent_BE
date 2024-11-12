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

            Include(u => u.User!);
            Include(u => u.Event!);
        }
    }
}
